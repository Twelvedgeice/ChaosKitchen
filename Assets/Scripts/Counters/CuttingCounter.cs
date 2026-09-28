using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IkitchenObjecParent, IHasProgress
{
    /*public event EventHandler<OnProgressChangedEventAarry> OnProgressChanged;
    public class OnProgressChangedEventAarry : EventArgs
    {
        public float progressNormalized;
    }*/

    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public event EventHandler <IHasProgress.OnProgressChangedEventAarry> OnProgressChanged;

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public int cuttingPrograss;
    public override void Interact(Player player)
    {
        if (!HaskKitchenObject())
        {
            // 柜子上没有物品
            if (player.HaskKitchenObject())
            {
                //Debug.Log(player.HaskKitchenObject());
                //Debug.Log(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()));
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()) || KitchenObjectPlacementRules.MustBePlacedOnPlateOrInPot(player.GetKitchenObject().GetKitchenObjectSO()) || player.GetKitchenObject().GetKitchenObjectSO().name == "CheeseBlock")
                {
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(player.GetKitchenObject().GetKitchenObjectSO());
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingPrograss = 0;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventAarry
                    {
                        progressNormalized = 0f
                    });
                }
                // 角色有物品，放置物品               
            }
            else
            {
                Debug.Log(player.GetKitchenObject());
                // 角色没有物品
            }
        }
        else
        {
            // 柜子上有物品
            if (player.HaskKitchenObject())
            {
                // 角色拿着盘子时，可直接收取切好的食材
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //该物品是盘子
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestorySelf();
                    }

                }
            }
            else
            {
                // 角色没有物品，拾取物品
                GetKitchenObject().SetKitchenObjectParent(player);
                this.ClearKitchenObject();
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HaskKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingPrograss++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventAarry
            {
                progressNormalized = (float)cuttingPrograss / cuttingRecipeSO.cuttingProgrossMax
            }); 
            if (cuttingPrograss >= cuttingRecipeSO.cuttingProgrossMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetInputForOutput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestorySelf();
                KitchenObject.SwpanKitchenObject(outputKitchenObjectSO, this);
            }          
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputkitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetInputForOutput(KitchenObjectSO inputkitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputkitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputkitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}
