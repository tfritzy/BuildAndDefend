﻿using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetLocationFlyingProjectile
{
    Vector3 TargetPosition { get; set; }

    void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        Vector3 targetPosition,
        float movementSpeed);
}