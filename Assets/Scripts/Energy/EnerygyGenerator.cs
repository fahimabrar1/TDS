using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnerygyGenerator : MonoBehaviour
{


    public int totalEnergyCount = 0; // total food currency
    public float energyGenerationTime = 5f; // Initial time for currency generation
    public float timeRemaining; // Time remaining for the next currency generation
    public Image progressBar; // Reference to the progress bar UI
    public TMP_Text energyCounter; // Reference to the rate text UI

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        timeRemaining = energyGenerationTime;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        UpdateProgressBar();
        GenerateFood();
    }

    void UpdateProgressBar()
    {
        timeRemaining -= Time.deltaTime;

        progressBar.fillAmount = 1 - (timeRemaining / energyGenerationTime);
    }

    void GenerateFood()
    {
        if (timeRemaining <= 0)
        {
            // Reset the timer only once
            timeRemaining = energyGenerationTime;
            // Generate food here
            totalEnergyCount++;

            energyCounter.text = totalEnergyCount.ToString();

            EnergyManager.instance.OnUpdateButtonsAction(totalEnergyCount);

        }
    }



    internal void DeductCurrency(int cost)
    {
        totalEnergyCount -= cost;
        EnergyManager.instance.OnUpdateButtonsAction(totalEnergyCount);
        energyCounter.text = totalEnergyCount.ToString();
    }
}
