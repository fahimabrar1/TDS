using UnityEngine;

[System.Serializable]
public class BaseUpgradeData : IApplyUpgradable
{

    public float defaultValue = 0;
    public float valueIncrementBy = 0;
    public int defaultCost = 0;
    public int costIncrementBy = 0;

    public int level = 1;

    public void ApplyUpgrade()
    {
        MyDebug.Log($"Apply Upgrade {nameof(BaseUpgradeData)}");
        defaultValue += valueIncrementBy;
        defaultCost += costIncrementBy;
        level++;
    }


    public float GetValue()
    {
        return Mathf.Round(defaultValue * 100f) / 100f;
    }


    public float GetValueRounded()
    {
        return Mathf.RoundToInt(defaultValue);
    }

    public int GetUpgradeCost()
    {
        return defaultCost;
    }


    public virtual void ResetData()
    {
    }
}