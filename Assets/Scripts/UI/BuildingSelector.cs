using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{

    public BuildingType Type;

    public void NotifyBuilderOfSelection()
    {
        Builder.SelectedBuilding = Builder.Buildings[this.Type];
    }
}