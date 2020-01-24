using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player PlayerData;
    public PlayerDataDAO Values;
    public static PlayerDataDAO Data
    {
        get
        {
            return Player.PlayerData.Values;
        }
    }

    void Awake()
    {
        if (PlayerData == null)
        {
            PlayerData = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (PlayerData != this)
        {
            Destroy(this.gameObject);
        }
        load();
    }

    private const string fileName = "PlayerData.json";
    private void load()
    {
        string fullPath = $"{Application.persistentDataPath}/{fileName}";
        Debug.Log(fullPath);
        if (File.Exists(fullPath))
        {
            StreamReader reader = new StreamReader(fullPath, true);
            string jsonPlayerData = reader.ReadToEndAsync().Result;
            if (string.IsNullOrEmpty(jsonPlayerData))
            {
                reader.Close();
                CreateNewSaveFile();
                return;
            }
            reader.Close();
            Player.PlayerData.Values = JsonConvert.DeserializeObject<PlayerDataDAO>(jsonPlayerData);
        }
        else
        {
            CreateNewSaveFile();
        }
    }

    private void CreateNewSaveFile()
    {
        Player.PlayerData.Values = new PlayerDataDAO();
        Save();
    }

    public void Save()
    {
        string serializedObject = JsonConvert.SerializeObject(Player.PlayerData.Values).ToString();
        string fullPath = $"{Application.persistentDataPath}/{fileName}";
        StreamWriter writer = new StreamWriter(fullPath, false);
        writer.Write(serializedObject);
        writer.Close();
    }


}