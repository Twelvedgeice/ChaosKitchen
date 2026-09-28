using UnityEngine.UI;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTamplate;
    public float customerWaitingTimer;

    private void Awake()
    {
        recipeTamplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
        DeliveryManager.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeComplete()
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawn()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        /*for (int i = 0; i < container.transform.childCount; i++)
        {
            Transform tamp = container.transform.GetChild(i);
            if (tamp == recipeTamplate)
            {
                continue;
            }
            Destroy(tamp.gameObject);
        }*/

        foreach (Transform child in container)
        {
            if (child == recipeTamplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        for (int i = 0; i < DeliveryManager.Instance.GetwaitingRecipeSOList().Count; i++)
        {
            var date = DeliveryManager.Instance.GetwaitingRecipeSOList()[i];
            var creat = Instantiate(recipeTamplate, container);
            creat.gameObject.SetActive(true);
            creat.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(date, customerWaitingTimer);
            //RecipeImage.sprite = date.sprite;
        }

    }
}
