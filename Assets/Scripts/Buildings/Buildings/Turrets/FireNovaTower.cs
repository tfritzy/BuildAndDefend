using UnityEngine;

public class FireNovaTower : AutoAttackTower
{
    public override TowerType Type => TowerType.FireNova;
    private int numProjectiles = 18;
    private float offset;
    protected float damageTickGap;

    protected override GameObject CreateProjectile(UnityEngine.Vector2 fireDirection)
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject instProj = Instantiate(
                Resources.Load<GameObject>($"{FilePaths.Projectiles}/{projectilePrefabName}"),
                this.transform.position,
                new Quaternion()
            );
            instProj.GetComponent<Rigidbody2D>().velocity = new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * (360f / (float)numProjectiles) * (float)i),
                Mathf.Sin(Mathf.Deg2Rad * (360f / (float)numProjectiles) * (float)i)) * (float)this.ProjectileMovementSpeed;
            SetProjectileValues(instProj);
        }
        return null;
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
        this.FireCooldown = 4f;
        this.ProjectileLifespan = 1.5f;
        this.damageTickGap = .5f;
        this.Range = 1.5f;
    }
}