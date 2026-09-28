using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpwan;
    public event EventHandler OnPlateRemove;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HaskKitchenObject())
        {
            OnPlateRemove?.Invoke(this, EventArgs.Empty);
            KitchenObject.SwpanKitchenObject(plateKitchenObjectSO, player);
        }
    }

    public override void InteractAlternate(Player player)
    {

    }
}
