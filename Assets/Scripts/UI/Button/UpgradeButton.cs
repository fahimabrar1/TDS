using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IApplyUpgradable
{
    public void ApplyUpgrade();
}


[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{

    public enum UpgradeValueType
    {
        fixedValue,
        rate,
    }

    public Button button;

    public TMP_Text levelText;
    public TMP_Text costText;
    public TMP_Text valueText;

    public UpgradeValueType upgradeValueType;
    public IApplyUpgradable applyUpgradable;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (TryGetComponent(out Button btn))
            button = btn;
    }


    public void ToggleButtonInteractable(bool value)
    {
        button.interactable = value;
    }

    public void ApplyUpgrade()
    {
        applyUpgradable.ApplyUpgrade();
    }

    internal void Initialize(BaseUpgradeData upgradeData)
    {
        if (levelText != null)
            levelText.text = $"Lv {upgradeData.level}";
        costText.text = upgradeData.defaultCost.ToString();
        valueText.text = upgradeValueType == UpgradeValueType.fixedValue ? upgradeData.GetValueRounded().ToString() : $"{upgradeData.GetValue()}/s";
    }

}
