using UnityEngine;
using UnityEngine.UI;

public class UpgradeTreeButton : MonoBehaviour
{
    public TowerType Type;
    public static GameObject UpgradePane;
    public GameObject ParentUpgradeTreeButton;
    public bool LevelValuesSelected = true;

    void Start()
    {
        SetUpgradeTreeButtonValues();
    }

    public void OpenUpgradePane()
    {
        CloseUpgradePanes();
        GameObject upgradePane = Resources.Load<GameObject>($"{FilePaths.UI}/UpgradeDescriptorPane");
        GameObject upgradePaneInst = GameObject.Instantiate(
            upgradePane,
            this.transform.position + Vector3.back,
            new Quaternion(),
            GameObjectCache.Canvas.transform);
        UpgradePane = upgradePaneInst;
        SetUpgradePaneValues();
        upgradePaneInst.transform.SetAsLastSibling();
    }

    public void RefreshValues()
    {
        SetUpgradePaneValues();
        SetUpgradeTreeButtonValues();
    }

    private void SetUpgradePaneValues()
    {
        UpgradePane.transform.Find("LevelUpButton").gameObject
            .GetComponent<PurchaseUpgradeButton>().Type = this.Type;
        UpgradePane.transform.Find("LevelUpButton").gameObject
            .GetComponent<PurchaseUpgradeButton>().ParentUpgradeTreeButton = this;
        UpgradePane.transform.Find("PowerUpButton").gameObject
            .GetComponent<PurchaseUpgradeButton>().Type = this.Type;
        UpgradePane.transform.Find("PowerUpButton").gameObject
            .GetComponent<PurchaseUpgradeButton>().ParentUpgradeTreeButton = this;
        UpgradePane.transform.Find("Title").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Building>().Name;
        UpgradePane.transform.Find("Upgrades").Find("UpgradeTitle").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Tower>().Stats.FieldsToString();
        UpgradePane.transform.Find("Upgrades").Find("CurrentStats").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Tower>().Stats.ToString();
        if (this.LevelValuesSelected)
        {
            SetLevelUpValues();
        }
        else
        {
            SetPowerUpValues();
        }

    }

    private void SetLevelUpValues()
    {
        UpgradePane.transform.Find("Cost").Find("Costs").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Building>().LevelUpCost.ToString();
        UpgradePane.transform.Find("Upgrades").Find("TargetStats").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Tower>().GetUpgradeStats(1, 0).ToString();
    }

    private void SetPowerUpValues()
    {
        UpgradePane.transform.Find("Cost").Find("Costs").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Building>().PowerUpCost.ToString();
        UpgradePane.transform.Find("Upgrades").Find("TargetStats").gameObject
            .GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Tower>().GetUpgradeStats(0, 1).ToString();
    }


    private void SetUpgradeTreeButtonValues()
    {
        // If this script is on an object that isn't an upgradeTreeButtton.
        if (!this.transform.Find("Tier"))
        {
            return;
        }
        this.transform.Find("Tier").gameObject.GetComponent<Text>().text = Player.Data.vals.BuildingUpgrades[Type].Tier.ToString();
        this.transform.Find("Level").gameObject.GetComponent<Text>().text = Player.Data.vals.BuildingUpgrades[Type].Level.ToString();
        this.transform.Find("Name").gameObject.GetComponent<Text>().text = GameObjectCache.Buildings[Type].GetComponent<Building>().Name;
    }

    public void CloseUpgradePanes()
    {
        if (UpgradePane != null)
        {
            Destroy(UpgradePane);
            UpgradePane = null;
        }
    }
}
