using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDeliveredText;
    [SerializeField] private TextMeshProUGUI numberOfGoldCoinsObtainedText;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button reStartButton;
    [SerializeField] private Button nextButton;

    private void Awake()
    {
        reStartButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.GetGameSceneNumber(KitchenGameManager.Instance.levelNumber));
        });

        returnButton.onClick.AddListener(() =>
        {
            Loader.Scene scene = Loader.GetGameSceneNumber(0);
            Loader.Load(scene);
        });

        nextButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            numberOfGoldCoinsObtainedText.text = DeliveryManager.Instance.GetSuccessfulRecipesGetSpeciesNumber().ToString();
        }
        else
        {
            Hide();
        }
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
