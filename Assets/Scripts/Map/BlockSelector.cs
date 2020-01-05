using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{

    public EnvironmentTileType Type;

    private Dictionary<EnvironmentTileType, GameObject> tileMap;

    void Start()
    {
        tileMap = new Dictionary<EnvironmentTileType, GameObject>();
        foreach (EnvironmentTileType type in Enum.GetValues(typeof(EnvironmentTileType)))
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