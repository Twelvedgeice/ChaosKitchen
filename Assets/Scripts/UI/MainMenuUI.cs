using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            GameDateManager.Instance.LevelNumber = 1;
            Loader.Load(Loader.Scene.GameScene1);
           
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;
        if (backgroundSprite != null)
        {
            var background = new GameObject("MainMenuBackground", typeof(RectTransform), typeof(Image));
            background.transform.SetParent(transform, false);
            background.transform.SetAsFirstSibling();
            var rect = background.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            var image = background.GetComponent<Image>();
            image.sprite = backgroundSprite;
            image.raycastTarget = false;
        }
    }
}

