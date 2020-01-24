using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public BuildingType BuildingType;

    void Start()
    {
        if (!Player.PlayerData.Values.BuildingUpgrades.ContainsKey(BuildingType))
        {
            BuildingDAO upgrade = new BuildingDAO(BuildingType);
            if (upgrade == null)
            {
                Debug.LogError($"Upgrade for building named '{this.BuildingType}' does not exist.");
            }
            Player.PlayerData.Values.BuildingUpgrades.Add(upgrade.Type, upgrade);
        }
    }
}