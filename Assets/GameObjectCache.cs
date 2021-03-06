using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectCache
{

    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }

    private static GameObject _canvas;
    public static GameObject Canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = GameObject.Find("Canvas");
            }
            return _canvas;
        }
    }

    private static Dictionary<BuildingType, GameObject> _buildings;
    public static Dictionary<BuildingType, GameObject> Buildings
    {
        get
        {
            if (_buildings == null)
            {
                _buildings = new Dictionary<BuildingType, GameObject>();
                foreach (BuildingType type in Enum.GetValues(typeof(BuildingType)))
                {
                    GameObject building = Resources.Load<GameObject>($"{FilePaths.Buildings}/{type}");
                    building.GetComponent<Building>().SetStats();
                    if (building != null)
                    {
                        _buildings.Add(type, building);
                    }
                }
            }
            return _buildings;
        }
    }
}