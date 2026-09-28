using UnityEngine;
using System;
using TMPro;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjeactSO;
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private TextMeshProUGUI numberText;

    private void Awake()
    {
        UpdateVisual();
    }
    public override void Interact(Player player)
    {
        if (!player.HaskKitchenObject() && kitchenObjeactSO.number > 0 )
        {
            //Debug.Log(HaskKitchenObject());
            KitchenObject.SwpanKitchenObject(kitchenObjeactSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            kitchenObjeactSO.number--;
            UpdateVisual();
        }

    }

    public override void InteractAlternate(Player player)
    {

    }

    private void UpdateVisual()
    {
        numberText.text = kitchenObjeactSO.number.ToString();
    }

}
