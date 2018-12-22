using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    List<ZombieSpawner> spawners;
    private float prevSpawnRate;
    private float waveStartTime;
    private bool isDuringWave; 

    public float waveDuration;
    public float timeBetweenWaves = 30f;
    public GameObject waveStartBanner;
    public int waveGroupCount = 5;

	// Use this for initialization
	void Start () {
        this.spawners = new List<ZombieSpawner>();
        GameObject[] objSpawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach(GameObject spawner in objSpawners)
        {
            spawners.Add(spawner.GetComponent<ZombieSpawner>());
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (isDuringWave)
        {
            if (Time.time > waveStartTime + waveDuration)
            {
                EndWave();
            }
        } else
        {
            if (Time.time > waveStartTime + timeBetweenWaves)
            {
                StartWave();
            }
        }
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
