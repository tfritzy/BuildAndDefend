using UnityEngine;
using System.Collections.Generic;

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

    private static Transform _towerSelectButtonParent;
    public static Transform TowerSelectButtonsParent
    {
        get
        {
            if (_towerSelectButtonParent == null)
            {
                _towerSelectButtonParent = GameObjectCache.Canvas.transform.Find("TowerSelectButtonsParent");
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

        float buttonWidth = selectTowerButton.GetComponent<RectTransform>().rect.width;
        float buttonHeight = selectTowerButton.GetComponent<RectTransform>().rect.height;
        float parentHeight = TowerSelectButtonsParent.GetComponent<RectTransform>().rect.height;
        float buttonYPos = (parentHeight / 2 - buttonHeight / 2 - 5 - ((buttonHeight + 5) * TowerSelectButtons.Count))
                           * GameObjectCache.Canvas.transform.localScale.y;
        Vector3 buttonPosition = new Vector2(0, buttonYPos);

        GameObject newButton = GameObject.Instantiate(
            selectTowerButton,
            TowerSelectButtonsParent.position + buttonPosition,
            new Quaternion(),
            TowerSelectButtonsParent.transform);
        newButton.GetComponent<TowerControlSelect>().selectType = type;
        newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = type.ToString();
        TowerSelectButtons.Add(type, newButton);
    }
}