using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EnergyGenerator : MonoBehaviour
{
    [Tooltip("Total energy count.")]
    public int totalEnergyCount = 0;

    [Tooltip("Initial time for energy generation.")]
    public float energyGenerationTime = 5f;

    [Tooltip("Time remaining for the next energy generation.")]
    private float timeRemaining;

    [Tooltip("Reference to the progress bar UI.")]
    public Image progressBar;

    [Tooltip("Reference to the energy count UI.")]
    public TMP_Text energyCounter;




    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        StartCoroutine(WaitForHealthData());
    }


    IEnumerator WaitForHealthData()
    {

        while (GameManager.instance == null || GameManager.instance.energyGenerateData == null)
        {
            yield return null; // Wait until healthData is assigned
        }
        GameManager.instance.energyGenerateData.OnUpdateDDefaultValue += OnUpddateDelay;
        OnUpddateDelay();
        totalEnergyCount = 0;
        timeRemaining = energyGenerationTime;

        UpdateEnergyText();

    }


    private void OnUpddateDelay()
    {
        energyGenerationTime = 1 / GameManager.instance.energyGenerateData.DefaultValue;
    }



    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        if (GameManager.instance != null && GameManager.instance.energyGenerateData != null)
        {
            GameManager.instance.energyGenerateData.OnUpdateDDefaultValue -= OnUpddateDelay;
        }
    }




    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        UpdateProgressBar();
    }

    /// <summary>
    /// Updates the progress bar.
    /// </summary>
    void UpdateProgressBar()
    {
        // Decrease the remaining time
        timeRemaining -= Time.deltaTime;

        // Update the progress bar fill
        progressBar.fillAmount = 1 - (timeRemaining / energyGenerationTime);

        // If the progress bar is full (fillAmount == 1), generate energy
        if (timeRemaining <= 0)
        {
            GenerateEnergy();
        }
    }

    /// <summary>
    /// Generates energy.
    /// </summary>
    void GenerateEnergy()
    {
        // Reset the timer
        timeRemaining = energyGenerationTime;

        // Increment energy count
        totalEnergyCount++;
        UpdateEnergyText();

        // Notify the EnergyManager (if needed)
        EnergyManager.instance.OnUpdateButtonsAction(totalEnergyCount);

        // Reset the progress bar fill to 0
        progressBar.fillAmount = 0;
    }

    /// <summary>
    /// Deducts currency from the total energy count.
    /// </summary>
    /// <param name="cost">The amount of energy to deduct.</param>
    internal void DeductCurrency(int cost)
    {
        totalEnergyCount -= cost;
        EnergyManager.instance.OnUpdateButtonsAction(totalEnergyCount);
        UpdateEnergyText();
    }

    /// <summary>
    /// Uses energy from the total energy count.
    /// </summary>
    /// <param name="energyUsed">The amount of energy used.</param>
    internal void OnUseEnergy(int energyUsed)
    {
        totalEnergyCount -= energyUsed;
        UpdateEnergyText();
        EnergyManager.instance.OnUpdateButtonsAction(totalEnergyCount);
    }

    /// <summary>
    /// Updates the energy text display.
    /// </summary>
    public void UpdateEnergyText()
    {
        energyCounter.text = totalEnergyCount.ToString();
    }
}