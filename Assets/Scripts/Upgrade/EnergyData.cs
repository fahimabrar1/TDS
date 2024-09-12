[System.Serializable]
public class EnergyGenerateData
{
    public float defaulEnergyRate = 0.2f;    // The default health value
    public float energyRateIncrement = .02f;  // The amount by which health increases on upgrade
    public int defaultEnergyRateCost = 50;       // The cost of upgrading the health
    public int energyRateCostIncrement = 50;       // The cost of upgrading the health

    // Constructor to initialize HealthData with default values
    public EnergyGenerateData()
    {
    }


    // Constructor to initialize HealthData with default values
    public EnergyGenerateData(float defaulEnergyRate, int defaultEnergyRateCost)
    {

        this.defaulEnergyRate = defaulEnergyRate;
        this.defaultEnergyRateCost = defaultEnergyRateCost;
    }

    // Method to apply an upgrade, increases health by healthIncrement
    public void ApplyUpgrade()
    {
        defaulEnergyRate += energyRateIncrement;
        defaultEnergyRateCost += energyRateCostIncrement;
    }

    // Method to get the current health
    public float GetEnergyRate()
    {
        return defaulEnergyRate;
    }

    // Method to get the cost of upgrading the health
    public int GetUpgradeCost()
    {
        return defaultEnergyRateCost;
    }


    // Method to get the cost of upgrading the health
    public void ResetData()
    {
        defaulEnergyRate = 0.2f;    // The default health value
        defaultEnergyRateCost = 50;       // The cost of upgrading the health
    }
}
