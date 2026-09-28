using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
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
    }
}

