using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public TowerType BuildingType;

    void Start()
    {
        if (!Player.Data.vals.BuildingUpgrades.ContainsKey(BuildingType))
        {
            BuildingDAO upgrade = new BuildingDAO(BuildingType);
            if (upgrade == null)
            {
                Debug.LogError($"Upgrade for building named '{this.BuildingType}' does not exist.");
            }
            Player.Data.vals.BuildingUpgrades.Add(upgrade.Type, upgrade);
        }
    }
}