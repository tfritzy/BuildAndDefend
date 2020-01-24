using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float prevSpawnRate;
    private float waveStartTime;
    private bool isDuringWave;
    private int numWaves;
    private bool duringNight = false;

    public float levelStartTime;
    public float waveDuration = 2f;
    public float timeBetweenWaves = 25f;
    public GameObject waveStartBanner;
    public int waveGroupCount = 2;
    public float levelDuration = 120f;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        EndNight();
    }

    // Update is called once per frame
    void Update()
    {
        ManageWaves();
    }

    public static void ReturnToMap()
    {
        SceneManager.LoadScene("Map");
    }

    public static void ShowWinScreen()
    {
        GameObject winWindow = Resources.Load<GameObject>(FilePaths.UI + "/WinWindow");
        Instantiate(winWindow, Vector3.zero, new Quaternion(), GameObject.Find("Canvas").transform);
    }

    public static void ShowLoseScreen()
    {
        GameObject winWindow = Resources.Load<GameObject>(FilePaths.UI + "/LoseWindow");
        Instantiate(winWindow, Vector3.zero, new Quaternion(), GameObject.Find("Canvas").transform);
    }

    void ManageWaves()
    {
        if (!duringNight)
        {
            return;
        }
        if (Time.time > levelStartTime + levelDuration)
        {
            EndWave();
            EndNight();
            Player.PlayerData.Save();
            ShowWinScreen();
            return;
        }
        if (isDuringWave)
        {
            if (Time.time > waveStartTime + waveDuration)
            {
                EndWave();
            }
        }
        else
        {
            if (Time.time > waveStartTime + timeBetweenWaves)
            {
                StartWave();
            }
        }
    }

    // Stops all zombies from being spawned because it is day!
    void EndNight()
    {
        foreach (GameObject zombie in GameObject.FindGameObjectsWithTag("Zombie"))
        {
            Destroy(zombie);
        }

        foreach (GameObject spawner in Map.Spawners.Values)
        {
            spawner.GetComponent<ZombieSpawner>().disabled = true;
        }
        duringNight = false;
    }

    void StartNight()
    {
        if (Map.BuildingsDict.Count == 0)
        {
            Debug.Log("You can't start a level with no towers!");
            return;
        }
        foreach (GameObject spawner in Map.Spawners.Values)
        {
            spawner.GetComponent<ZombieSpawner>().disabled = false;
        }
        numWaves = Random.Range(2, 5);
        timeBetweenWaves = levelDuration / numWaves;
        levelStartTime = Time.time;
        duringNight = true;
    }

    void StartWave()
    {
        waveStartTime = Time.time;
        List<GameObject> spawners = Map.Spawners.Values.ToList();
        prevSpawnRate = spawners[0].GetComponent<ZombieSpawner>().SpawnRate;
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].GetComponent<ZombieSpawner>().SpawnZombie(waveGroupCount);
            spawners[i].GetComponent<ZombieSpawner>().SpawnRate /= 4;
        }

        isDuringWave = true;
    }

    void EndWave()
    {
        List<GameObject> spawners = Map.Spawners.Values.ToList();
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].GetComponent<ZombieSpawner>().SpawnRate = prevSpawnRate;
        }
        isDuringWave = false;
    }
}
