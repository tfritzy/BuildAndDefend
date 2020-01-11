using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetLocationSpawningProjectile
{
    float SpawnDelay { get; set; }

    void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        float spawnDelay,
        Vector2 targetPosition);
}
