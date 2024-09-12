using UnityEngine;
using UnityEngine.UI;

public class LevelMenuUI : MonoBehaviour
{

    public UpgradeButton healthUpgradeButton; // Example button for upgrading
    public UpgradeButton energyUpgradeButton; // Example button for upgrading



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


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        healthUpgradeButton.Initialize(GameManager.instance.healthData);
        energyUpgradeButton.Initialize(GameManager.instance.energyGenerateData);
    }


    // This method is called whenever coins change
    private void UpdateUpgradeButton()
    {
        if (CurrencyManager.Instance != null && GameManager.instance.healthData != null)
        {

            // Enable the button if enough coins, otherwise disable
            healthUpgradeButton.ToggleButtonInteractable(CurrencyManager.Instance.Coins >= GameManager.instance.healthData.defaultCost);
            energyUpgradeButton.ToggleButtonInteractable(CurrencyManager.Instance.Coins >= GameManager.instance.energyGenerateData.defaultCost);
        }
    }

    // Upgrade method, called when the button is pressed
    public void OnUpgradeButtonPressed()
    {
        if (CurrencyManager.Instance != null && GameManager.instance.healthData != null && CurrencyManager.Instance.Coins >= GameManager.instance.healthData.defaultCost)
        {
            int cost = GameManager.instance.healthData.defaultCost;
            healthUpgradeButton.ApplyUpgrade();

            CurrencyManager.Instance.SpendCoins(cost);

            MyDebug.Log("Upgrade successful.");
        }
        else
        {
            MyDebug.Log("Not enough coins for upgrade.");
        }
    }


}
