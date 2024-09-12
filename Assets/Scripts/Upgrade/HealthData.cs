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
    public HealthData(int defaultValue, int defaultCost)
    {

        this.defaultValue = defaultValue;
        this.defaultCost = defaultCost;
    }


    // Method to get the cost of upgrading the health
    public override void ResetData()
    {
        defaultValue = 500;
        valueIncrementBy = 100;
        defaultCost = 150;
        costIncrementBy = 50;

    }

}
