using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image Barimage;
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += hasProgress_OnProgressChaned;
        Hide();
        Barimage.fillAmount = 0f;
    }

    private void hasProgress_OnProgressChaned(object sender, IHasProgress.OnProgressChangedEventAarry e)
    {
        Barimage.fillAmount = e.progressNormalized;

        if(e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
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
