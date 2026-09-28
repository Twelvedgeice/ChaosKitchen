using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarmingUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventAarry e)
    {
        float burnProgressNormalized = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnProgressNormalized;
        if (show)
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
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
