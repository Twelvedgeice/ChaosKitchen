using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackageObjectSingleUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Transform packageObjectTemlate;
    [SerializeField] private Image packageObjectIconImage;
    [SerializeField] private TextMeshProUGUI packageObjectNumberText;

    public void SetPackageObject(Transform iconTransform, KitchenObjectSO kitchenObjectSO)
    {
        packageObjectIconImage.GetComponent<Image>().sprite  = kitchenObjectSO.sprite;
        packageObjectNumberText.text = kitchenObjectSO.number.ToString();
    }
}
