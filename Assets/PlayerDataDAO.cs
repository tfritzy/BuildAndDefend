using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerDataDAO
{
    public int Gold;
    public int Iron;
    public int Wood;
    public int Stone;

    public List<string> BeatenLevels;

    public string CurrentLevel;

    private Dictionary<TowerType, BuildingDAO> _buildingUpgrades;

    public Dictionary<TowerType, BuildingDAO> BuildingUpgrades
    {
        get
        {
            if (_buildingUpgrades == null)
            {
                _buildingUpgrades = new Dictionary<TowerType, BuildingDAO>();
            }
            return _buildingUpgrades;
        }
        set
        {
            _buildingUpgrades = value;
        }
    }
}