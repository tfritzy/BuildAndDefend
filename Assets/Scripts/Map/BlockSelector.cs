using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{

    public TileType Type;

    private Dictionary<TileType, GameObject> tileMap;

    void Start()
    {
        tileMap = new Dictionary<TileType, GameObject>();
        foreach (TileType type in Enum.GetValues(typeof(TileType)))
        {
            Debug.Log($"{FilePaths.Terrain}/{type}");
            tileMap.Add(type, Resources.Load<GameObject>($"{FilePaths.Terrain}/{type}"));
        }
    }

    public void NotifyMapBuilderOfSelection()
    {
        MapBuilder.SelectedTileType = Type;
        MapBuilder.SelectedBlock = tileMap[Type];
    }
}