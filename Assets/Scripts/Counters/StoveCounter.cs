using System;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventAarry> OnProgressChanged;
    public class OnStateChangedEventArgs : EventArgs { public State state; }
    public enum State { Idle, Frying, Fried, Burned }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private readonly List<KitchenObjectSO> combinedIngredients = new List<KitchenObjectSO>();
    private readonly List<GameObject> combinedIngredientVisuals = new List<GameObject>();
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;
    private State state;

    private void Start() { state = State.Idle; }

    private void Update()
    {
        if (!HaskKitchenObject()) return;
        if (state == State.Frying)
        {
            fryingTimer += Time.deltaTime;
            SendProgress(fryingTimer / fryingRecipeSO.FryingTimerMax);
            if (fryingTimer >= fryingRecipeSO.FryingTimerMax)
            {
                GetKitchenObject().DestorySelf();
                KitchenObject.SwpanKitchenObject(fryingRecipeSO.output, this);
                combinedIngredients.Clear();
                combinedIngredients.Add(GetKitchenObject().GetKitchenObjectSO());
                state = State.Fried;
                burningTimer = 0f;
                burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                SendState();
                SendProgress(0f);
            }
        }
        else if (state == State.Fried && burningRecipeSO != null)
        {
            burningTimer += Time.deltaTime;
            SendProgress(burningTimer / burningRecipeSO.BurningTimerMax);
            if (burningTimer >= burningRecipeSO.BurningTimerMax)
            {
                GetKitchenObject().DestorySelf();
                KitchenObject.SwpanKitchenObject(burningRecipeSO.output, this);
                combinedIngredients.Clear();
                combinedIngredients.Add(GetKitchenObject().GetKitchenObjectSO());
                ClearCombinedIngredientVisuals();
                state = State.Burned;
                SendState();
                SendProgress(0f);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.HaskKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject heldPlate)
            && heldPlate.GetKitchenObjectSOList().Count > 0)
        {
            PourPlate(heldPlate);
            return;
        }
        if (!HaskKitchenObject()) { StartCooking(player); return; }
        if (player.HaskKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate)) ServeToPlate(plate);
            else AddIngredient(player);
            return;
        }

        if (combinedIngredients.Count <= 1)
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            ClearKitchenObject();
            combinedIngredients.Clear();
            ClearCombinedIngredientVisuals();
            ResetCookingState();
        }
    }

    public override void InteractAlternate(Player player) { }

    private void StartCooking(Player player)
    {
        if (!player.HaskKitchenObject()) return;
        var food = player.GetKitchenObject().GetKitchenObjectSO();
        if (IsPrepared(food))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            combinedIngredients.Clear();
            combinedIngredients.Add(food);
            state = State.Fried;
            burningTimer = 0f;
            burningRecipeSO = GetBurningRecipeSOWithInput(food);
            SendState(); SendProgress(0f);
            return;
        }
        FryingRecipeSO recipe = GetFryingRecipeSOWithInput(food);
        if (recipe == null) return;
        fryingRecipeSO = recipe;
        player.GetKitchenObject().SetKitchenObjectParent(this);
        combinedIngredients.Clear();
        ClearCombinedIngredientVisuals();
        state = State.Frying;
        fryingTimer = 0f;
        SendState();
        SendProgress(0f);
    }

    private void AddIngredient(Player player)
    {
        KitchenObjectSO potFood = GetKitchenObject().GetKitchenObjectSO();
        if (!IsPrepared(potFood)) return;

        if (state != State.Fried)
        {
            state = State.Fried;
            burningRecipeSO = GetBurningRecipeSOWithInput(potFood);
            SendState();
        }

        KitchenObjectSO ingredient = player.GetKitchenObject().GetKitchenObjectSO();
        if (!IsPrepared(ingredient) || ContainsFood(ingredient)) return;

        if (combinedIngredients.Count == 0) combinedIngredients.Add(potFood);
        combinedIngredients.Add(ingredient);
        if (ingredient.name == "RiceCooked") burningRecipeSO = GetBurningRecipeSOWithInput(ingredient);
        CreateCombinedIngredientVisual(ingredient);
        player.GetKitchenObject().DestorySelf();
        burningTimer *= .5f;
        SendProgress(burningRecipeSO == null ? 0f : burningTimer / burningRecipeSO.BurningTimerMax);
    }

    private void ServeToPlate(PlateKitchenObject plate)
    {
        KitchenObjectSO potFood = GetKitchenObject().GetKitchenObjectSO();
        if (!IsPrepared(potFood) || plate.GetKitchenObjectSOList().Count != 0) return;
        if (combinedIngredients.Count == 0) combinedIngredients.Add(potFood);
        foreach (KitchenObjectSO ingredient in combinedIngredients)
            if (!plate.TryAddIngredient(ingredient)) return;

        GetKitchenObject().DestorySelf();
        combinedIngredients.Clear();
        ClearCombinedIngredientVisuals();
        ResetCookingState();
    }

    private void CreateCombinedIngredientVisual(KitchenObjectSO ingredient)
    {
        Transform visual = Instantiate(ingredient.prefab, GetKitchenObjectFollowTransform());
        visual.name = "Mixed_" + ingredient.name;
        int index = combinedIngredientVisuals.Count;
        float angle = index * 2.1f;
        visual.localPosition = new Vector3(Mathf.Cos(angle) * .22f, .18f + index * .035f, Mathf.Sin(angle) * .22f);
        visual.localRotation = Quaternion.Euler(0f, index * 55f, 0f);
        visual.localScale = Vector3.one * .55f;
        KitchenObject kitchenObject = visual.GetComponent<KitchenObject>();
        if (kitchenObject != null) kitchenObject.enabled = false;
        combinedIngredientVisuals.Add(visual.gameObject);
    }

    private void ClearCombinedIngredientVisuals()
    {
        foreach (GameObject visual in combinedIngredientVisuals)
            if (visual != null) Destroy(visual);
        combinedIngredientVisuals.Clear();
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO recipe in fryingRecipeSOArray) if (recipe.input == input) return recipe;
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (BurningRecipeSO recipe in burningRecipeSOArray) if (recipe.input == input) return recipe;
        return null;
    }

    private void ResetCookingState()
    {
        state = State.Idle;
        fryingTimer = 0f;
        burningTimer = 0f;
        fryingRecipeSO = null;
        burningRecipeSO = null;
        SendState();
        SendProgress(0f);
    }

    private void SendState() => OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    private void SendProgress(float progress) => OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventAarry { progressNormalized = Mathf.Clamp01(progress) });
    public bool IsFried() { return state == State.Fried; }
    private static bool IsPrepared(KitchenObjectSO food)
    {
        return food != null && (food.name == "RiceCooked" || KitchenObjectPlacementRules.CanCombineWithCookedRice(food));
    }
    private bool ContainsFood(KitchenObjectSO food)
    {
        if (HaskKitchenObject() && GetKitchenObject().GetKitchenObjectSO().name == food.name) return true;
        return combinedIngredients.Exists(item => item.name == food.name);
    }
    private void PourPlate(PlateKitchenObject plate)
    {
        if (HaskKitchenObject() && !IsPrepared(GetKitchenObject().GetKitchenObjectSO())) return;
        var incoming = new List<KitchenObjectSO>(plate.GetKitchenObjectSOList());
        if (incoming.Count == 0 || incoming.Exists(food => !IsPrepared(food))) return;
        var additions = incoming.FindAll(food => !ContainsFood(food));
        if (additions.Count == 0) return;
        if (HaskKitchenObject() && combinedIngredients.Count == 0)
            combinedIngredients.Add(GetKitchenObject().GetKitchenObjectSO());
        foreach (var food in additions)
        {
            if (!HaskKitchenObject()) KitchenObject.SwpanKitchenObject(food, this);
            else CreateCombinedIngredientVisual(food);
            combinedIngredients.Add(food);
            if (food.name == "RiceCooked") burningRecipeSO = GetBurningRecipeSOWithInput(food);
            burningTimer *= .5f;
        }
        plate.RemoveIngredients(additions);
        state = State.Fried;
        SendState();
        SendProgress(burningRecipeSO == null ? 0f : burningTimer / burningRecipeSO.BurningTimerMax);
    }}
