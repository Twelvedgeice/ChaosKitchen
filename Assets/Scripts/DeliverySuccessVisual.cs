using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliverySuccessVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addSpeciesNumberText;
    private int SpeciesNumber;

    private void Start()
    {
        Hide();
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
    }

    private void DeliveryManager_OnDeliverySuccess(object sender,DeliveryManager.OnDeliverySuccessEventArgs e)
    {
        SpeciesNumber = e.successRecipeSO.price ;
        DateShowUI.instance.SetSpeciesNumber(SpeciesNumber);
        DateShowUI.instance.UpdateVisual();
        Debug.Log(SpeciesNumber);
        Show();
        Invoke("Hide", 5f);
    }

    private void Show()
    {
        UpdateVisual();
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        addSpeciesNumberText.text = "+" + SpeciesNumber.ToString();
    }
}
