using UnityEngine;
using System;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private int coins;

    public Action OnCurrencyChanged;

    public TMP_Text coinText;  // TextMeshPro text component to display coins

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        OnCurrencyChanged += UpdateCoinText; // Corrected typo
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        OnCurrencyChanged -= UpdateCoinText; // Corrected typo
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadCurrency();

        }
        else
        {
            Destroy(gameObject); // Destroy the GameObject, not just this component
        }
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        OnCurrencyChanged?.Invoke();  // Ensure the UI updates immediately after loading the currency
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(GameConstants.COIN, 0);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(GameConstants.COIN, Coins);
        PlayerPrefs.Save();
    }

    // Property to safely manipulate coins
    public int Coins
    {
        get => coins;
        private set
        {
            coins = value;
            SaveCurrency();
            OnCurrencyChanged?.Invoke();  // Notify listeners when coins change
        }
    }

    // Add coins and notify
    public void AddCoins(int amount = 1)
    {
        Coins += amount;
    }

    // Spend coins only if sufficient
    public void SpendCoins(int amount = 1)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
        }
        else
        {
            MyDebug.Log("Not enough coins.");
        }
    }

    // Method to update the UI text when coins change
    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = Coins.ToString();
        }
        else
        {
            MyDebug.LogWarning("Coin text reference is missing!");
        }
    }
}
