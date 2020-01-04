using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour
{
    public static bool isMenuOpen;
    public string mapName;
    public MapRewardsDAO rewards;
    private GameObject mapDetailsPane;

    void Start()
    {
        this.mapDetailsPane = Resources.Load<GameObject>($"{FilePaths.UI}/MapDetailsPane");
        Load();
    }

    public void Load()
    {
        string path = $"{FilePaths.Maps}/{this.name}.json";
        StreamReader reader = new StreamReader(path);
        string jsonMap = reader.ReadToEnd();
        MapDAO map = JsonConvert.DeserializeObject<MapDAO>(jsonMap);
        this.rewards = map.rewards;
    }

    public void ShowLevelDetails()
    {
        Player.data.vals.CurrentLevel = this.mapName;
        Instantiate(
            this.mapDetailsPane,
            this.transform.position + new Vector3(1, 0),
            new Quaternion(),
            this.transform);
        isMenuOpen = true;
    }
}
