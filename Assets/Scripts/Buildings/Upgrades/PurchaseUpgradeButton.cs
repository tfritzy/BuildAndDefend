using UnityEngine;
using UnityEngine.UI;

public class PurchaseUpgradeButton : MonoBehaviour
{
    public UpgradeTreeButton ParentUpgradeTreeButton;
    public TowerType Type;
    public bool OnConfirmationStage = false;
    private Color originalColor;
    private string originalText;

    public void BuyLevelUp()
    {
        if (!OnConfirmationStage)
        {
            ParentUpgradeTreeButton.LevelValuesSelected = true;
            SetConfirmationValues();
        }
        else
        {
            Player.Data.vals.BuildingUpgrades[Type].BuyLevelUp();
            GameObjectCache.Buildings[Type].GetComponent<Building>().SetStats();
            SetBaseValues();
        }
    }

    public void BuyPowerUp()
    {
        if (!OnConfirmationStage)
        {
            ParentUpgradeTreeButton.LevelValuesSelected = false;
            SetConfirmationValues();
        }
        else
        {
            Player.Data.vals.BuildingUpgrades[Type].BuyPowerUp();
            GameObjectCache.Buildings[Type].GetComponent<Building>().SetStats();
            SetBaseValues();
        }
    }

    private void SetConfirmationValues()
    {
        this.originalColor = this.GetComponent<Button>().image.color;
        this.originalText = this.GetComponentInChildren<Text>().text;
        this.GetComponent<Button>().image.color = Color.green;
        this.GetComponentInChildren<Text>().text = "Purchase";
        this.OnConfirmationStage = true;
        ParentUpgradeTreeButton.RefreshValues();
    }

    private void SetBaseValues()
    {
        ParentUpgradeTreeButton.RefreshValues();
        this.GetComponent<Button>().image.color = originalColor;
        this.GetComponentInChildren<Text>().text = originalText;
        this.OnConfirmationStage = false;
    }
}