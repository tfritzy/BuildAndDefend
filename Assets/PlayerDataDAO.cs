using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerDataDAO
{
    public int Gold;
    public int Iron;
    public int Wood;
    public int Stone;
    public int SkillPoints;

    public List<string> BeatenLevels;

    public string CurrentLevel;

    private Dictionary<BuildingType, BuildingDAO> _buildingUpgrades;

    public Dictionary<BuildingType, BuildingDAO> BuildingUpgrades
    {
        get
        {
            if (_buildingUpgrades == null)
            {
                _buildingUpgrades = new Dictionary<BuildingType, BuildingDAO>();
            }
            foreach (BuildingType type in Enum.GetValues(typeof(BuildingType)))
            {
                if (!_buildingUpgrades.ContainsKey(type))
                {
                    _buildingUpgrades.Add(type, new BuildingDAO(type));
                }
            }
            return _buildingUpgrades;
        }
        set
        {
            _buildingUpgrades = value;
        }
    }

    /// <summary>
    /// The resource collection rate of each map in units per hour.
    /// key = mapName, value = resourcesPerHour
    /// </summary>
    public Dictionary<string, ResourceDAO> ResourceHarvestByMapPerHour
    {
        get
        {
            if (_resourceHarvestByMapPerHour == null)
            {
                _resourceHarvestByMapPerHour = new Dictionary<string, ResourceDAO>();
            }
            return _resourceHarvestByMapPerHour;
        }
        set
        {
            _resourceHarvestByMapPerHour = value;
        }
    }

    private Dictionary<string, ResourceDAO> _resourceHarvestByMapPerHour;
}