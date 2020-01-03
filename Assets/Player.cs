using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player data;

    public PlayerDataDAO vals;

    void Awake()
    {
        if (data == null)
        {
            data = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (data != this)
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
            reader.Close();
            data.vals = JsonConvert.DeserializeObject<PlayerDataDAO>(jsonPlayerData);
        }
        else
        {
            Player.data.vals = new PlayerDataDAO();
            save();
        }
    }

    public async void save()
    {
        string fullPath = $"{Application.persistentDataPath}/{fileName}";
        StreamWriter writer = new StreamWriter(fullPath, false);
        await writer.WriteAsync(JsonConvert.SerializeObject(data.vals).ToString());
        writer.Close();
    }


}