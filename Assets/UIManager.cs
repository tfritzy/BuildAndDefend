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
        int i = 1;
        foreach (BuildingType tower in FactionTowers.Towers[Faction.Fire])
        {
            CreateTowerButton(tower, parent, i);
            i += 1;
        }
    }

    private void CreateTowerButton(BuildingType towerType, GameObject parent, int i)
    {
        GameObject towerButton = Resources.Load<GameObject>($"{FilePaths.UI}/UpgradeTreeButton");
        Vector2 position = new Vector2((i % 10) * 1.1f - 5f, (i / 10) * 1.1f);
        GameObject button = Instantiate(towerButton, position, new Quaternion(), parent.transform);
        button.GetComponent<UpgradeTreeButton>().Type = towerType;
        button.GetComponent<UpgradeTreeButton>().ParentUpgradeTreeButton = button;
    }
}
