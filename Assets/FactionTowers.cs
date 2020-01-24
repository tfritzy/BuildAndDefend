

using System.Collections.Generic;
using UnityEngine;

public static class FactionTowers
{
    private static Dictionary<Faction, HashSet<BuildingType>> _towers;
    public static Dictionary<Faction, HashSet<BuildingType>> Towers
    {
        get
        {
            if (_towers == null)
            {
                _towers = new Dictionary<Faction, HashSet<BuildingType>>();
                foreach (GameObject building in GameObjectCache.Buildings.Values)
                {
                    if (!_towers.ContainsKey(building.GetComponent<Building>().Faction))
                    {
                        _towers.Add(building.GetComponent<Building>().Faction, new HashSet<BuildingType>());
                    }
                    _towers[building.GetComponent<Building>().Faction].Add(building.GetComponent<Building>().Type);
                }
            }
            return _towers;
        }
    }
}