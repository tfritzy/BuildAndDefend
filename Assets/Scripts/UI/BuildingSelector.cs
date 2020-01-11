using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{

    public TowerType Type;

    public void NotifyBuilderOfSelection()
    {
        Builder.SelectedBuilding = Builder.Buildings[this.Type];
    }
}