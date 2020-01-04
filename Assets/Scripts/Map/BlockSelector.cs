using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{

    public TileType Type;

    public void NotifyMapBuilderOfSelection() { }

    private Dictionary<TileType, GameObject> tileMap;


    void Start()
    {

    }

    public void NotifyMapBuilderOfSelection()
    {
        MapBuilder.SelectedTileType = Type;
        MapBuilder.Selecte
    }
}