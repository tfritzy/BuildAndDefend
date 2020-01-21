using System;
using UnityEngine;

public abstract class TargetLocationTower : Tower
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
            this.transform.position,
            new Quaternion()
        );
        SetProjectileValues(instProj, input);
        return instProj;
    }

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        p.GetComponent<ITargetLocationProjectile>().SetParameters(
            Stats.Damage,
            Stats.ProjectileLifespan,
            Stats.Pierce,
            this,
            Stats.ExplosionRadius,
            ((VectorInputDAO)input).location.Value);
    }
}