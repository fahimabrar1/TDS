using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private int coins;

    public event Action OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            LoadCurrency();
        }
        else
        {
            Destroy(gameObject); // Destroy the GameObject, not just this component
        }
    }

    private void LoadCurrency()
    {
        coins = PlayerPrefs.GetInt(GameConstants.COIN, 0);
        OnCurrencyChanged?.Invoke();  // Notify UI of currency changes
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(GameConstants.COIN, coins);
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
        MyDebug.Log("Coins added.");
    }

    // Spend coins only if sufficient
    public void SpendCoins(int amount = 1)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            MyDebug.Log("Coins spent.");
        }
        else
        {
            MyDebug.Log("Not enough coins.");
        }
    }

}
