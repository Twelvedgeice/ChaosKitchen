using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public delegate void OnRecipeSpawn ();
public delegate void OnRecipeComplete();


public class DeliveryManager : MonoBehaviour
{
    public OnRecipeSpawn OnRecipeSpawn;
    public OnRecipeComplete OnRecipeComplete;


    public event EventHandler<OnDeliverySuccessEventArgs> OnDeliverySuccess;
    public class OnDeliverySuccessEventArgs : EventArgs
    {
        public RecipeSO successRecipeSO;
    }
    public event EventHandler OnDeliveryFail;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer = 30f;
    private const float spawnRecipeTimerMax = 30f;
    private int waitRecipeSOMax = 4;

    private int successfulRecipesAmount = 0;
    private int successfulRecipesGetSpeciesNumber = 0;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        DeliveryManagerSingleUI.SetIsDeleted();
    }

    //更新菜谱
    private void Update()
    {
        if (KitchenGameManager.Instance == null || !KitchenGameManager.Instance.IsGamePlaying()) return;
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitRecipeSOMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawn?.Invoke();               
            }
        }

        DelateFirst();
    }

    //耐心耗尽
    private void DelateFirst()
    {
        if (DeliveryManagerSingleUI.isDeleted)
        {
            DeliveryManagerSingleUI.SetIsDeleted();
            if (waitingRecipeSOList.Count > 0)
            {
                waitingRecipeSOList.RemoveAt(0);
                OnRecipeComplete?.Invoke();
            }

        }
    }

    //判断提交的菜是否对应菜单
    public void DeliceRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // 订单中的菜品与送上去的菜品由同样数量的物品组成
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // 遍历订单中的菜品所组成的物品
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // 遍历送上去的菜品所组成的物品
                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            // 订单中的菜品与送上去的菜品由同样的物品组成
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // 订单中的菜品与送上去的菜品由不同的物品组成
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                   // Debug.Log("Player delivered the correct recipe!");
                    waitingRecipeSOList.RemoveAt(i);
                    successfulRecipesAmount++;
                    successfulRecipesGetSpeciesNumber += waitingRecipeSO.price;
                    OnRecipeComplete?.Invoke();                        

                    OnDeliverySuccess?.Invoke(this, new OnDeliverySuccessEventArgs {
                        successRecipeSO = waitingRecipeSO
                    });
                    return;
                }
            }
        }
        // 遍历了所有订单，没有找到匹配的订单
        //Debug.Log("Player did not deliver a correct recipe!");
        OnDeliveryFail?.Invoke(this, EventArgs.Empty);
    
    }

    public List<RecipeSO> GetwaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }

    public int GetSuccessfulRecipesGetSpeciesNumber()
    {
        return successfulRecipesGetSpeciesNumber;
    }
}
