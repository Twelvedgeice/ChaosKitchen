using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnKitchenObjectChanged();
//public delegate void OnkitchenObjectChang
public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjeactSO;    

    public override void Interact(Player player)
    {
        if (!HaskKitchenObject())
        {
            // 柜子上没有物品
            if (player.HaskKitchenObject())
            {
                if (KitchenObjectPlacementRules.MustBePlacedOnPlateOrInPot(
                    player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    return;
                }

                // 角色有物品，放置物品
                player.GetKitchenObject().SetKitchenObjectParent(this);
                //player.ClearKitchenObject();
            }
            else
            {
                // 角色没有物品
            }
        }
        else
        {
            // 柜子上有物品
            if (player.HaskKitchenObject())
            {
                // 角色有物品
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Debug.Log(player.GetKitchenObject().TryGetPlate(out plateKitchenObject));
                    //该物品是盘子
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestorySelf();
                    }

                }
                else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestorySelf();
                        }
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

    }

}
