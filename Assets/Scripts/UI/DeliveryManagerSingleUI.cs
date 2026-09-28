using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTamplate;
    [SerializeField] private Image progressTimerImage;
    [SerializeField] private Image RecipeImage;

    private float customerWaitingTimer;
    private float customerWaitingTimerMax;

    public static bool isDeleted = false;
    private void Awake()
    {
        iconTamplate.gameObject.SetActive(false);
        customerWaitingTimerMax = GameDateManager.instance.GetpatienceTimer(KitchenGameManager.Instance.levelNumber);
    }

    public void SetRecipeSO(RecipeSO recipeSO, float customerWaitingTimer)
    {
        RecipeImage.sprite = recipeSO.sprite;
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTamplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTamplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }

    private void Update()
    {
        customerWaitingTimer += Time.deltaTime;
        progressTimerImage.fillAmount = 1 - customerWaitingTimer / customerWaitingTimerMax;
        if(progressTimerImage.fillAmount <= 0 && !isDeleted)
        {
            isDeleted = true;
        }
    }

    public static void SetIsDeleted()
    {
        isDeleted = false;
    }
}
