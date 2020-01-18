using System;
using UnityEngine;

public abstract class TargetLocationSpawningTower : Tower
{
    protected float explosionDelay;

    protected override void Fire(InputDAO input)
    {
        if (!(input is VectorInputDAO))
        {
            throw new ArgumentException("Input was not the correct type.");
        }

        CreateProjectile(input);
        lastFireTime = Time.time;
    }

    protected override GameObject CreateProjectile(InputDAO input)
    {
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{projectilePrefabName}"),
            ((VectorInputDAO)input).location.Value,
            new Quaternion()
        );
        SetProjectileValues(instProj, input);
        return instProj;
    }

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        p.GetComponent<ITargetLocationProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius,
            ((VectorInputDAO)input).location.Value);
    }
}