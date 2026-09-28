using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameobjectArray;

    private void Start()
    {
        //添加一个响应事件
        Player.Instance.OnSelectCounterChanged += Player_OnSelectCounterChanged;
    }

    private void Player_OnSelectCounterChanged(object sender, Player.OnSelectCounterChangedEventArgs e)
    {
        if(e.baseCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameobjectArray)
        {
            visualGameObject.SetActive(true);
        }

    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameobjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
