using UnityEngine;

public abstract class TargetLocationTower : Tower
{
    protected float explosionDelay;
    protected Vector3 lastInputPosition;

    protected override void Fire()
    {
        Vector2? inputLocation = GetInputLocation();
        if (inputLocation.HasValue)
        {
            lastInputPosition = inputLocation.Value;
            CreateProjectile(inputLocation.Value);
            lastFireTime = Time.time;
        }
    }

    protected override void CreateProjectile(UnityEngine.Vector2 fireDirection)
    {
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{projectilePrefabName}"),
            this.transform.position,
            new Quaternion()
        );
        SetProjectileValues(instProj);
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