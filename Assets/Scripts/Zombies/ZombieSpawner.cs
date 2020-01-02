using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {

    public float averageBetweenZombies;
    public GameObject zombie;
    public bool disabled = false;

    private float lastZombieSpawnTime;
    private float timeBetweenZombie;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!disabled && Time.time > timeBetweenZombie + lastZombieSpawnTime)
        {
            SpawnZombie(1);
            timeBetweenZombie = Random.Range(averageBetweenZombies / 2, averageBetweenZombies * 2);
            lastZombieSpawnTime = Time.time;
        }
	}

    public void SpawnZombie(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float xShake = Random.Range(-.2f, .2f);
            float yShake = Random.Range(-.2f, .2f);
            Vector2 locationShake = new Vector2(xShake, yShake);
            Instantiate(zombie, (Vector2)this.transform.position + locationShake, new Quaternion());
        }
    }
}
