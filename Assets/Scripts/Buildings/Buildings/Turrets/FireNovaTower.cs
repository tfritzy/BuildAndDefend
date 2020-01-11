using UnityEngine;

public class FireNovaTower : TargetLocationTower
{
    public override TowerType Type => TowerType.FireNova;
    private int numProjectiles = 36;
    private float offset;

    protected override void CreateProjectile(UnityEngine.Vector2 fireDirection)
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject instProj = Instantiate(
                Resources.Load<GameObject>($"{FilePaths.Projectiles}/{projectilePrefabName}"),
                lastInputPosition,
                new Quaternion()
            );
            instProj.GetComponent<Rigidbody2D>().velocity = new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * (360f / (float)numProjectiles) * (float)i),
                Mathf.Sin(Mathf.Deg2Rad * (360f / (float)numProjectiles) * (float)i)) * (float)this.projectileSpeed;
            Projectile p = instProj.GetComponent<Projectile>();
            SetProjectileValues(p);
        }
    }

    protected override void SetProjectileValues(Projectile p)
    {
        p.GetComponent<Projectile>().SetParameters(
            this.projectileDamage,
            this.projectileLifespan,
            this.projectilePierce,
            this
        );
    }

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.projectileDamage = 10;
        this.inaccuracy = .1f;
        this.projectileSpeed = .7f;
        this.fireCooldown = .3f;
        this.projectileLifespan = .8f;
        this.explosionRadius = 1f;
        this.projectilePierce = 10;
    }
}