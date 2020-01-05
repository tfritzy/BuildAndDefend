using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    /// <summary>
    /// This spawner's spawn rate in zombies per second.
    /// </summary>
    public float SpawnRate;

    /// <summary>
    /// The zombie to spawn.
    /// </summary>
    public GameObject zombie;

    /// <summary>
    /// Whether or not this spawner is spawning.
    /// </summary>
    public bool disabled = true;

    private float lastZombieSpawnTime;
    private float timeBetweenZombie;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled && Time.time > timeBetweenZombie + lastZombieSpawnTime)
        {
            SpawnZombie(1);
            timeBetweenZombie = Random.Range(SpawnRate / 2, SpawnRate * 2);
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
