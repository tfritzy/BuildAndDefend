using System.Collections.Generic;
using UnityEngine;
using System;

public interface IConstantDamageProjectile
{
    float DamageTickGapInSeconds { get; set; }

    void SetParameters(
        int damage,
        float lifespan,
        Tower owner,
        float damageTickGapInSeconds);
}