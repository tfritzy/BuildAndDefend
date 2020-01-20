using UnityEngine;

public class PurchaseUpgradeButton : MonoBehaviour
{
    public UpgradeTreeButton ParentUpgradeTreeButton;
    public TowerType Type;

    public void BuyLevelUp()
    {
        Player.Data.vals.BuildingUpgrades[Type].BuyLevelUp();
        ParentUpgradeTreeButton.RefreshValues();
    }

    public void BuyPowerUp()
    {
        Player.Data.vals.BuildingUpgrades[Type].BuyPowerUp();
        ParentUpgradeTreeButton.RefreshValues();
    }
}