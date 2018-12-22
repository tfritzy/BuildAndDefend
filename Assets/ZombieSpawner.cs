using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {

    public float averageBetweenZombies;
    public GameObject zombie;

    private float lastZombieSpawnTime;
    private float timeBetweenZombie;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeBetweenZombie + lastZombieSpawnTime)
        {
            Instantiate(zombie, this.transform.position, new Quaternion());
            
            timeBetweenZombie = Random.Range(averageBetweenZombies / 2, averageBetweenZombies * 2);
            lastZombieSpawnTime = Time.time;
        }
	}

    public void SpawnZombie(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(zombie, this.transform.position, new Quaternion());
        }
    }
}
