using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITimedExplosionProjectile
{
    float ExplosionDelay { get; set; }

    void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        float spawnDelay);
}
