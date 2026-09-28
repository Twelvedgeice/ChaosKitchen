using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryFailedVisual : MonoBehaviour
{
    private void Start()
    {
        Hide();
        DeliveryManager.Instance.OnDeliveryFail += DeliveryManager_OnDeliveryFail;
    }

    private void DeliveryManager_OnDeliveryFail(object sender, System.EventArgs e)
    {
        Show();
        Invoke("Hide", 1f);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
