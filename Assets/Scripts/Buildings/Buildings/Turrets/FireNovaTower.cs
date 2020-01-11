using UnityEngine;

public class FireNovaTower : TargetLocationTower
{
    public override TowerType Type => TowerType.FireNova;
    private int numProjectiles = 36;
    private float offset;
    protected float damageTickGap;

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
                Mathf.Sin(Mathf.Deg2Rad * (360f / (float)numProjectiles) * (float)i)) * (float)this.ProjectileMovementSpeed;
            SetProjectileValues(instProj);
        }
    }

    protected override void SetProjectileValues(GameObject p)
    {
        p.GetComponent<IConstantDamageProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this,
            this.damageTickGap);
    }

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 10;
        this.inaccuracy = .1f;
        this.ProjectileMovementSpeed = .7f;
        this.FireCooldown = .3f;
        this.ProjectileLifespan = .8f;
        this.damageTickGap = .5f;
    }
}