using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    List<ZombieSpawner> spawners;
    private float prevSpawnRate;
    private float waveStartTime;
    private bool isDuringWave;
    private int numWaves;
    private bool duringNight = false;
    // private Darkness darkness;

    public float levelStartTime;
    public float waveDuration;
    public float timeBetweenWaves = 30f;
    public GameObject waveStartBanner;
    public int waveGroupCount = 5;
    public float levelDuration = 240f;

    private const string UIPrefabsPath = "Gameobjects/UI";

    private void Awake()
    {
        //  Disabling Darkness for now
        //this.darkness = GameObject.Find("Night").GetComponent<Darkness>();

    }

    // Use this for initialization
    void Start () {
        this.spawners = new List<ZombieSpawner>();
        GameObject[] objSpawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach(GameObject spawner in objSpawners)
        {
            spawners.Add(spawner.GetComponent<ZombieSpawner>());
        }
        EndNight();
	}
	
	// Update is called once per frame
	void Update () {
        ManageWaves();
    }

    public static void ReturnToMap(){
        SceneManager.LoadScene("Map");
    }

    public static void ShowWinScreen(){
        GameObject winWindow = Resources.Load<GameObject>(UIPrefabsPath + "/WinWindow");
        Instantiate(winWindow, Vector3.zero, new Quaternion(), GameObject.Find("Canvas").transform);
    }

    public static void ShowLoseScreen(){
        GameObject winWindow = Resources.Load<GameObject>(UIPrefabsPath + "/LoseWindow");
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
            return;
        }
        if (isDuringWave)
        {
            if (Time.time > waveStartTime + waveDuration)
            {
                EndWave();
                ShowWinScreen();
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
        foreach (ZombieSpawner spawner in spawners)
        {
            spawner.disabled = true;
        }
        duringNight = false;
        // darkness.EndNight();
        
    }

    void StartNight()
    {
        foreach (ZombieSpawner spawner in spawners)
        {
            spawner.disabled = false;
        }
        numWaves = Random.Range(2, 5);
        timeBetweenWaves = levelDuration / numWaves;
        levelStartTime = Time.time;
        duringNight = true;
        // darkness.StartNight();
    }

    void StartWave()
    {
        Instantiate(waveStartBanner, GameObject.Find("Canvas").transform);
        waveStartTime = Time.time;
        prevSpawnRate = spawners[0].averageBetweenZombies;
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].SpawnZombie(waveGroupCount);
            spawners[i].averageBetweenZombies /= 4;
        }
        
        isDuringWave = true;
    }

    void EndWave()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].averageBetweenZombies = prevSpawnRate;
        }
        isDuringWave = false;
    }
}
