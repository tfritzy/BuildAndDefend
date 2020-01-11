using UnityEngine;

public abstract class TargetLocationTower : ExplosiveProjectileTower
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
            lastInputPosition,
            new Quaternion()
        );
        Projectile p = instProj.GetComponent<Projectile>();
        SetProjectileValues(p);
    }
}