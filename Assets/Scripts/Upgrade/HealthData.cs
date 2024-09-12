[System.Serializable]
public class HealthData
{
    public int defaultHealth = 500;    // The default health value
    public int healthIncrement = 100;  // The amount by which health increases on upgrade
    public int defaulthealthCost = 150;       // The cost of upgrading the health
    public int healthCostIncrement = 50;       // The cost of upgrading the health

    // Constructor to initialize HealthData with default values
    public HealthData()
    {
    }


    // Constructor to initialize HealthData with default values
    public HealthData(int defaultHealth, int defaulthealthCost)
    {

        this.defaultHealth = defaultHealth;
        this.defaulthealthCost = defaulthealthCost;
    }

    // Method to apply an upgrade, increases health by healthIncrement
    public void ApplyUpgrade()
    {
        defaultHealth += healthIncrement;
        defaulthealthCost += healthCostIncrement;
    }

    // Method to get the current health
    public int GetHealth()
    {
        return defaultHealth;
    }

    // Method to get the cost of upgrading the health
    public int GetUpgradeCost()
    {
        return defaulthealthCost;
    }


    // Method to get the cost of upgrading the health
    public void ResetData()
    {
        defaultHealth = 500;    // The default health value
        defaulthealthCost = 150;       // The cost of upgrading the health
    }
}
