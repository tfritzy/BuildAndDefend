using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControlSelect : MonoBehaviour
{
    public BuildingType selectType;

    public void SelectTowerType()
    {
        foreach (string key in Map.BuildingDict.Keys)
        {
            if (Map.BuildingDict[key].CompareTag(Tags.Tower))
            {
                if (Map.BuildingDict[key].GetComponent<Tower>().Type == selectType)
                {
                    Map.BuildingDict[key].GetComponent<Tower>().SetIsActive(true);
                }
                else
                {
                    Map.BuildingDict[key].GetComponent<Tower>().SetIsActive(false);
                }
            }
        }
    }
}
