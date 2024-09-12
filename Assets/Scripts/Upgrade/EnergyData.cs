[System.Serializable]
public class EnergyGenerateData : BaseUpgradeData
{

    // Constructor to initialize HealthData with default values
    public EnergyGenerateData()
    {
        ResetData();
    }


    // Constructor to initialize HealthData with default values
    public EnergyGenerateData(float defaultValue, int defaultCost)
    {
        this.defaultValue = defaultValue;
        this.defaultCost = defaultCost;
    }

    // Method to get the cost of upgrading the health
    public override void ResetData()
    {
        defaultValue = 0.2f;
        valueIncrementBy = 0.02f;
        defaultCost = 50;
        costIncrementBy = 50;

    }
}
