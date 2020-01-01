using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour, IPointerDownHandler, IBeginDragHandler
{
    public static bool isMenuOpen;
    public string mapName;
    public MapRewardsDAO rewards;
    private GameObject mapDetailsPane;
    private const string mapDetailsPanePath = "Gameobjects/UI/MapDetailsPane";
    private static string mapRootPath = "Assets/Maps/";
    void Start()
    {
        this.mapDetailsPane = Resources.Load<GameObject>(mapDetailsPanePath);
        Load();
    }

    public void Load()
    {
        string path = mapRootPath + this.name;
        StreamReader reader = new StreamReader(path);
        string jsonMap = reader.ReadToEnd();
        MapDAO map = JsonConvert.DeserializeObject<MapDAO>(jsonMap);
        this.rewards = map.rewards;
    }

    public void ShowLevelDetails()
    {
        Player.data.vals.currentLevel = this.mapName;
        Instantiate(
            this.mapDetailsPane,
            this.transform.position + new Vector3(1, 0),
            new Quaternion(),
            this.transform);
        isMenuOpen = true;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(eventData);
    }
}
