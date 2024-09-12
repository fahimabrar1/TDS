using UnityEngine;
using UnityEngine.UI;

public class LevelMenuUI : MonoBehaviour
{

    public Button healthUpgradeButton; // Example button for upgrading
    public Button energyUpgradeButton; // Example button for upgrading



    private void OnEnable()
    {
        // Subscribe to currency change events
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateUpgradeButton;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUpgradeButton;
        }
    }





    // This method is called whenever coins change
    private void UpdateUpgradeButton()
    {
        if (CurrencyManager.Instance != null && GameManager.instance.healthData != null)
        {

            // Enable the button if enough coins, otherwise disable
            healthUpgradeButton.interactable = CurrencyManager.Instance.Coins >= GameManager.instance.healthData.defaulthealthCost;
        }
    }

    // Upgrade method, called when the button is pressed
    public void OnUpgradeButtonPressed()
    {
        if (CurrencyManager.Instance != null && GameManager.instance.healthData != null && CurrencyManager.Instance.Coins >= GameManager.instance.healthData.defaulthealthCost)
        {
            CurrencyManager.Instance.SpendCoins(GameManager.instance.healthData.defaulthealthCost);
            MyDebug.Log("Upgrade successful.");
        }
        else
        {
            MyDebug.Log("Not enough coins for upgrade.");
        }
    }


}
