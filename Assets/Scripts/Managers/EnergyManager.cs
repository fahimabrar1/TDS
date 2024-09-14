using System;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [Tooltip("The singleton instance of the EnergyManager.")]
    public static EnergyManager instance;


    [Tooltip("The energy generator component.")]
    public EnergyGenerator energyGenerator;

    [Tooltip("The prefab for the ability buttons.")]
    public GameObject AbilityButtonPrefab;

    [Tooltip("The parent transform for the ability buttons.")]
    public Transform AbilityButtonParent;

    [Tooltip("The scriptable object that holds the ability list.")]
    public AbilityButtonHolderNewSO abilityButtonHolderNewSO;

    [Tooltip("A list of ability buttons.")]
    public List<AbilityButton> abilityButtons;


    [Tooltip("An action that is invoked when the ability buttons need to be updated.")]
    public Action<int> OnUpdateButtonsAction;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    /// <summary>
    /// Initializes the ability buttons.
    /// </summary>
    public void SpawnAbilityButtons()
    {
        abilityButtons = new();
        for (int i = 0; i < abilityButtonHolderNewSO.abilityList.Count; i++)
        {
            var ButtonObj = Instantiate(AbilityButtonPrefab, AbilityButtonParent);
            if (ButtonObj.TryGetComponent(out AbilityButton abilityButton))
            {
                abilityButton.Initialize(abilityButtonHolderNewSO.abilityList[i], energyGenerator);
                OnUpdateButtonsAction += abilityButton.OnButtonActiveCheck;
                abilityButtons.Add(abilityButton);
            }
        }
    }


    public void OnClickAbilityButton()
    {

    }




    /// <summary>
    /// Called when an energy button is clicked.
    /// </summary>
    /// <param name="energyUsed">The amount of energy used.</param>
    public void OnClickEnergyButton(int energyUsed, AbilityButton.AbilityButtonType abilityButtonType)
    {
        energyGenerator.OnUseEnergy(energyUsed);
        switch (abilityButtonType)
        {
            case AbilityButton.AbilityButtonType.crate:
                LevelManager.instance.GetPlayer().SpawnCrate();
                break;
            case AbilityButton.AbilityButtonType.fire:
                LevelManager.instance.GetPlayer().ActivateFirePowerdMode();
                break;
            case AbilityButton.AbilityButtonType.granade:
                LevelManager.instance.GetPlayer().ThrowGrenade();
                break;
        }
    }



    internal void DestroyAbilityButtons()
    {
        var childCount = abilityButtons.Count;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(abilityButtons[i].gameObject);
        }
    }
}