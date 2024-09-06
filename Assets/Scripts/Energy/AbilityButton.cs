using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AbilityButton : MonoBehaviour
{



    public Button button;
    public Image contentIcon;
    public TMP_Text costText;

    public bool canBuy;

    public int cost;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    public void SetOnUpdateButtonsAction()
    {
        EnergyManager.instance.OnUpdateButtonsAction += OnUpdateButtons;
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        EnergyManager.instance.OnUpdateButtonsAction -= OnUpdateButtons;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        canBuy = false;
        costText.text = cost.ToString();
    }




    private void OnUpdateButtons(int currencyValue)
    {
        if (currencyValue >= cost)
        {
            button.interactable = true;
            //Todo: make is purchasable
        }
        else
        {
            button.interactable = false;
        }
    }



    public void OnClickButton()
    {
        EnergyManager.instance.OnClickCurrencyButton(cost);
        // GameManager.instance.battleManager.playerbase.SpawnSoldier();
    }
}