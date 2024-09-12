using System;
using UnityEngine;

[System.Serializable]
public class BaseUpgradeData : IApplyUpgradable
{
    [SerializeField]
    private float defaultValue = 0;
    public float valueIncrementBy = 0;
    public int defaultCost = 0;
    public int costIncrementBy = 0;
    public int level = 1;
    public int levelIncrementBy = 0;

    public Action OnUpdateDDefaultValue;



    // Property to safely manipulate coins
    public float DefaultValue
    {
        get => defaultValue;
        protected set
        {
            defaultValue = value;
            OnUpdateDDefaultValue?.Invoke();  // Notify listeners when coins change
        }
    }


    public void ApplyUpgrade()
    {
        MyDebug.Log($"Apply Upgrade {nameof(BaseUpgradeData)}");
        DefaultValue += valueIncrementBy;
        defaultCost += costIncrementBy;
        level++;
    }


    public float GetValue()
    {
        return Mathf.Round(DefaultValue * 100f) / 100f;
    }


    public float GetValueRounded()
    {
        return Mathf.RoundToInt(DefaultValue);
    }

    public int GetUpgradeCost()
    {
        return defaultCost;
    }


    public virtual void ResetData()
    {
    }
}