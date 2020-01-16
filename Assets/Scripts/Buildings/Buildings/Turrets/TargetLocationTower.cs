using System;
using UnityEngine;

public abstract class TargetLocationTower : Tower
{
    protected float explosionDelay;
    protected Vector3 lastInputPosition;

    protected override void Fire(InputDAO input)
    {
        if (!(input is BasicInputDAO))
        {
            throw new ArgumentException("Input was not the correct type.");
        }

        CreateProjectile(((BasicInputDAO)input).location.Value);
        lastFireTime = Time.time;
    }

    protected override GameObject CreateProjectile(UnityEngine.Vector2 fireDirection)
    {
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{projectilePrefabName}"),
            fireDirection,
            new Quaternion()
        );
        SetProjectileValues(instProj);
        return instProj;
    }

    protected override void SetProjectileValues(GameObject p)
    {
        p.GetComponent<ITargetLocationProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius,
            this.lastInputPosition);
    }
}