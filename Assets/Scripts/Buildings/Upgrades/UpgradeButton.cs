using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public TowerType BuildingType;

    void Start()
    {
        if (!Player.Data.vals.BuildingUpgrades.ContainsKey(BuildingType))
        {
            BuildingDAO upgrade = GetDefaultUpgradeInstance(this.BuildingType);
            if (upgrade == null)
            {
                Debug.LogError($"Upgrade for building named '{this.BuildingType}' does not exist.");
            }
            Player.Data.vals.BuildingUpgrades.Add(upgrade.Type, upgrade);
        }
    }

    public void Upgrade()
    {
        Player.Data.vals.BuildingUpgrades[BuildingType].BuyUpgrade();
    }

    private BuildingDAO GetDefaultUpgradeInstance(TowerType buildingName)
    {
        switch (buildingName)
        {
            case (TowerType.Ballista):
                return new BallistaUpgrade();
            default:
                return null;
        }
    }
}