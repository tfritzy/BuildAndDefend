using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControlSelect : MonoBehaviour
{
    public TowerType selectType;

    public void SelectTowerType()
    {
        foreach (string key in Map.BuildingsDict.Keys)
        {
            if (Map.BuildingsDict[key].CompareTag(Tags.Tower))
            {
                if (Map.BuildingsDict[key].GetComponent<Tower>().Type == selectType)
                {
                    Map.BuildingsDict[key].GetComponent<Tower>().SetIsActive(true);
                }
                else
                {
                    Map.BuildingsDict[key].GetComponent<Tower>().SetIsActive(false);
                }
            }
        }
    }
}
