using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public BuildingType BuildingType;

    void Start(){
        if (!Player.data.vals.BuildingUpgrades.ContainsKey(BuildingType)){
            BuildingUpgrade upgrade = GetDefaultUpgradeInstance(this.BuildingType);
            if (upgrade == null){
                Debug.LogError($"Upgrade for building named '{this.BuildingType}' does not exist.");
            }
            Player.data.vals.BuildingUpgrades.Add(upgrade.Type, upgrade);
        }
    }

    public void Upgrade()
    {
        Player.data.vals.BuildingUpgrades[BuildingType].BuyUpgrade();
    }

    private BuildingUpgrade GetDefaultUpgradeInstance(BuildingType buildingName){
        switch(buildingName){
            case (BuildingType.Ballista):
                return new BallistaUpgrade();
            default:
                return null;
        }
    }
}