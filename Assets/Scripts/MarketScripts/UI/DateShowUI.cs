using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DateShowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speciesNumberText;
    [SerializeField] private Button packageButton;

    private int speciesNumber = GameDateManager.Instance.SPECIES_NUMBER;

    public static DateShowUI instance;

    private void Start()
    {
        if (instance == null)
            instance = this;

        UpdateVisual();

        if(PackageShowUI.instance == null)
        {
            PackageShowUI.instance = new PackageShowUI();
            Debug.Log(PackageShowUI.instance);
            PackageShowUI.instance.Hide();
        }
        
        packageButton.onClick.AddListener(() =>
        {
            PackageShowUI.instance.UPdataVisual();
            PackageShowUI.instance.Show();
        });
    }

/*    private void Update()
    {
        UpdateVisual();
    }*/

    public int GetSpeciesNumber()
    {
        return speciesNumber;
    }

    public void SetSpeciesNumber(int ChangedNumber)
    {
        speciesNumber += ChangedNumber;
        GameDateManager.Instance.SPECIES_NUMBER += ChangedNumber;
    }

    public void UpdateVisual()
    {
        speciesNumberText.text = speciesNumber.ToString();
    }

    public bool CompareSpeciesNumberWithSpendNumber(int SpendNumber)
    {
        return speciesNumber > SpendNumber;
    }
}
