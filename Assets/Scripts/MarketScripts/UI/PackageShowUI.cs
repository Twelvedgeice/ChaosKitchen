using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageShowUI : MonoBehaviour
{
    public static PackageShowUI instance;

    [SerializeField] Button closeButton;
    [SerializeField] Transform container;
    [SerializeField] Transform packageObjectTemlate;
    [SerializeField] private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

    }

    private void Start()
    {
        Hide();
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UPdataVisual()
    {
        foreach(Transform child in container)
        {
            if(child == packageObjectTemlate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in kitchenObjectSOList)
        {
            if(kitchenObjectSO.number > 0)
            {
                var creat = Instantiate(packageObjectTemlate, container);
                creat.gameObject.SetActive(true);
                creat.gameObject.GetComponent<PackageObjectSingleUI>().SetPackageObject(creat, kitchenObjectSO);
            }
        }
    }

}
