using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject canvas;
    void Start()
    {
        this.canvas = GameObject.Find("Canvas");
    }

    public void OpenUpgradeMenu()
    {
        GameObject upgradeWindow = Resources.Load<GameObject>($"{FilePaths.UI}/KineticUpgradeMenu");
        GameObject parent = Instantiate(upgradeWindow, Vector3.zero, new Quaternion(), this.canvas.transform);
        SetupUpgradeMenuButtons(parent);
    }

    private void SetupUpgradeMenuButtons(GameObject parent)
    {
        foreach (TowerType tower in FactionTowers.Towers[Faction.Fire])
        {
            CreateTowerButton(tower, parent);
        }
    }

    private void CreateTowerButton(TowerType towerType, GameObject parent)
    {
        GameObject towerButton = Resources.Load<GameObject>($"{FilePaths.UI}/UpgradeTreeButton");
        Vector2 position = new Vector2(UnityEngine.Random.Range(-7, 7), UnityEngine.Random.Range(-4, 4));
        GameObject button = Instantiate(towerButton, position, new Quaternion(), parent.transform);
        button.transform.Find("Name").gameObject.GetComponent<Text>().text = GameObjectCache.Buildings[towerType].GetComponent<Building>().Name;
        button.transform.Find("Tier").gameObject.GetComponent<Text>().text = Player.Data.vals.BuildingUpgrades[towerType].Level.ToString();
    }
}
