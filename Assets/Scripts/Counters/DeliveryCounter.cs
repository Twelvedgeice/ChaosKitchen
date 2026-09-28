using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        //if (Instance == null)
            Instance = this;
    }

    public override void Interact(Player player)
    {
        //Debug.Log("1111");
        //Debug.Log(DeliveryCounter.Instance);
        if (player.HaskKitchenObject())
        {
            Debug.Log(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchen1));
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchen))
            {
                DeliveryManager.Instance.DeliceRecipe(plateKitchen);
                player.GetKitchenObject().DestorySelf();
            }
        }
    }

    public override void InteractAlternate(Player player)
    {

    }
}
