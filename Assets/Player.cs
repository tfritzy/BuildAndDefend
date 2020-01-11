using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Data;

    public PlayerDataDAO vals;

    void Awake()
    {
        if (Data == null)
        {
            Data = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Data != this)
        {
            Destroy(this.gameObject);
        }
        load();
    }

    private const string fileName = "PlayerData.json";
    private void load()
    {
        string fullPath = $"{Application.persistentDataPath}/{fileName}";
        if (File.Exists(fullPath))
        {
            StreamReader reader = new StreamReader(fullPath, true);
            string jsonPlayerData = reader.ReadToEndAsync().Result;
            reader.Close();
            Data.vals = JsonConvert.DeserializeObject<PlayerDataDAO>(jsonPlayerData);
            LoadBuildingUpgradesWithInheritance();
        }
        else
        {
            Player.Data.vals = new PlayerDataDAO();
            Save();
        }
    }

    private void LoadBuildingUpgradesWithInheritance()
    {
        var buildingUpgradesWithInheritance = new Dictionary<TowerType, BuildingUpgrade>();

        foreach (TowerType type in Enum.GetValues(typeof(TowerType)))
        {
            if (Data.vals.BuildingUpgrades.ContainsKey(type))
            {
                buildingUpgradesWithInheritance[type] = Data.vals.BuildingUpgrades[type].GetInstance();
            }
            else
            {
                buildingUpgradesWithInheritance[type] = new BuildingUpgrade(type).GetInstance();
            }
        }

        foreach (TowerType type in Data.vals.BuildingUpgrades.Keys)
        {

        }
        Data.vals.BuildingUpgrades = buildingUpgradesWithInheritance;
    }

    public void Save()
    {
        string fullPath = $"{Application.persistentDataPath}/{fileName}";
        StreamWriter writer = new StreamWriter(fullPath, false);
        writer.Write(JsonConvert.SerializeObject(Player.Data.vals).ToString());
        writer.Close();
    }


}