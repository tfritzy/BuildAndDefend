using UnityEngine;

public interface ITargetEntityFlyingProjectile
{
    GameObject Target { get; set; }

    void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        GameObject target,
        float movementSpeed);
}