using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTreeButton : MonoBehaviour
{
    public TowerType Type;

    public void OpenUpgradePane()
    {
        GameObject upgradePane = Resources.Load<GameObject>($"{FilePaths.UI}/UpgradeDescriptorPane");
        GameObject inst = GameObject.Instantiate(upgradePane, this.transform.position, new Quaternion(), this.transform);
        inst.transform.Find("LevelUpButton").gameObject.GetComponent<UpgradeButton>().BuildingType = this.Type;
        inst.transform.Find("PowerUpButton").gameObject.GetComponent<UpgradeButton>().BuildingType = this.Type;
    }
}
