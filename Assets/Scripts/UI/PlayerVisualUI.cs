using UnityEngine;
using UnityEngine.UI;

public class PlayerVisualUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform iconShow;
    [SerializeField] private Image iconImage;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (player.HaskKitchenObject() && !player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            Show();
            iconImage.sprite = player.GetKitchenObject().GetKitchenObjectSO().sprite;
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
