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
    public BaseUpgradeData applyUpgradable;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (TryGetComponent(out Button btn))
            button = btn;

    }
    public void Initialize(BaseUpgradeData upgradeData)
    {
        applyUpgradable = upgradeData;
        UpdateButtonUI();
    }


    public void ToggleButtonInteractable(bool value)
    {
        button.interactable = value;
    }

    public void ApplyUpgrade()
    {
        MyDebug.Log("Apply Upgrade From  Button");
        applyUpgradable.ApplyUpgrade();
        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        if (levelText != null)
            levelText.text = $"Lv {applyUpgradable.level}";
        costText.text = applyUpgradable.defaultCost.ToString();
        valueText.text = upgradeValueType == UpgradeValueType.fixedValue ? applyUpgradable.GetValueRounded().ToString() : $"{applyUpgradable.GetValue()}/s";
    }


}
