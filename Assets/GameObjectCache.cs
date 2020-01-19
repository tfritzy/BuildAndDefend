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

    private static Dictionary<TowerType, GameObject> _buildings;
    public static Dictionary<TowerType, GameObject> Buildings
    {
        get
        {
            if (_buildings == null)
            {
                _buildings = new Dictionary<TowerType, GameObject>();
                foreach (TowerType type in Enum.GetValues(typeof(TowerType)))
                {
                    GameObject building = Resources.Load<GameObject>($"{FilePaths.Buildings}/{type}");
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