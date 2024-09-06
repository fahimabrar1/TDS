using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    [Tooltip("The type of ability button.")]
    public enum AbilityButtonType
    {
        crate,
        fire,
        granade
    }

    [Tooltip("The button component.")]
    public Button button;

    [Tooltip("The image component for the content icon.")]
    public Image contentIcon;

    [Tooltip("The text component for the cost.")]
    public TMP_Text costText;

    [Tooltip("The type of ability button.")]
    public AbilityButtonType abilityButtonType;


    [Tooltip("Whether the button is interactable.")]
    public bool interactable;

    [Tooltip("The cost of the ability.")]
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
        interactable = false;
        costText.text = cost.ToString();
    }

    /// <summary>
    /// Called when the button is clicked.
    /// </summary>
    public void OnClickButton()
    {
        EnergyManager.instance.OnClickEnergyButton(cost);
        // GameManager.instance.battleManager.playerbase.SpawnSoldier();
    }

    /// <summary>
    /// Initializes the ability button with the given ability button scriptable object.
    /// </summary>
    /// <param name="abilityButtonSO">The ability button scriptable object.</param>
    internal void Initialize(AbilityButtonSO abilityButtonSO)
    {
        cost = abilityButtonSO.cost;
        abilityButtonType = abilityButtonSO.abilityButtonType;
        contentIcon.sprite = abilityButtonSO.sprite;
    }

    /// <summary>
    /// Updates the button.
    /// </summary>
    public void UpdateButton()
    {
        costText.text = cost.ToString();
        interactable = false;
        button.interactable = interactable;
    }

    /// <summary>
    /// Called when the energy count is updated.
    /// </summary>
    /// <param name="totalEnergyCount">The total energy count.</param>
    public void OnButtonActiveCheck(int totalEnergyCount)
    {
        button.interactable = totalEnergyCount >= cost;
    }

    /// <summary>
    /// Called when the energy count is updated.
    /// </summary>
    /// <param name="energyValue">The current energy count.</param>
    private void OnUpdateButtons(int energyValue)
    {
        button.interactable = energyValue >= cost;
    }
}