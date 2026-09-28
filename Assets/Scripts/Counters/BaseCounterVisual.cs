using UnityEngine;
using UnityEngine.UI;

public class BaseCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private Transform iconShow;
    [SerializeField] private Image iconImage;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (baseCounter.HaskKitchenObject() && !baseCounter.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            Show();
            iconImage.sprite = baseCounter.GetKitchenObject().GetKitchenObjectSO().sprite;
        }
        else
        {
            Hide();
        }
        
    }

    private void Show()
    {
        iconShow.gameObject.SetActive(true);
        //iconImage.gameObject.SetActive(true);
    }

    private void Hide()
    {
        iconShow.gameObject.SetActive(false);
        //iconImage.gameObject.SetActive(false);
    }
}
