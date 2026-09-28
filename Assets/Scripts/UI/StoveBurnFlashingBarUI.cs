using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FFLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        animator.SetBool(IS_FFLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventAarry e)
    {
        float burnProgressNormalized = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnProgressNormalized;
        animator.SetBool(IS_FFLASHING, show);
    }

   
}
