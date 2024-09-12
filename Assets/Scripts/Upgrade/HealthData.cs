[System.Serializable]
public class HealthData : BaseUpgradeData
{

    // public UpgradeCostModel upgradeCostModel;
    // Constructor to initialize HealthData with default values
    public HealthData()
    {
        ResetData();
    }


    // Constructor to initialize HealthData with default values
    public HealthData(float defaultValue, int defaultCost)
    {

        DefaultValue = defaultValue;
        this.defaultCost = defaultCost;
    }


    // Method to get the cost of upgrading the health
    public override void ResetData()
    {
        DefaultValue = 500;
        valueIncrementBy = 100;
        defaultCost = 150;
        costIncrementBy = 50;

    }

}
