using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TowerSelectManager
{
    private static Dictionary<BuildingType, GameObject> _towerSelectButtons;
    public static Dictionary<BuildingType, GameObject> TowerSelectButtons
    {
        get
        {
            if (_towerSelectButtons == null)
            {
                _towerSelectButtons = new Dictionary<BuildingType, GameObject>();
            }
            return _towerSelectButtons;
        }
        set
        {
            _towerSelectButtons = value;
        }
    }

    private const string towerSelectButtonPrefabName = "TowerControlSelectButton";
    private static GameObject _towerSelectButton;
    private static GameObject selectTowerButton
    {
        get
        {
            if (_towerSelectButton == null)
            {
                _towerSelectButton = Resources.Load<GameObject>($"{FilePaths.UI}/{towerSelectButtonPrefabName}");
            }
            return _towerSelectButton;
        }
    }

    private static GameObject _towerSelectButtonParent;
    public static GameObject TowerSelectButtonsParent {
        get {
            if (_towerSelectButtonParent == null){
                _towerSelectButtonParent = Resources.Load<GameObject>("TowerSelectButtonsParent");
            }
            return _towerSelectButtonParent;
        }
    }

    public static void AddTowerButton(BuildingType type)
    {
        if (TowerSelectButtons.ContainsKey(type))
        {
            return;
        }

        Vector3 buttonPosition = Vector3.zero;

        GameObject.Instantiate(selectTowerButton, buttonPosition, new Quaternion(), null);
    }
}