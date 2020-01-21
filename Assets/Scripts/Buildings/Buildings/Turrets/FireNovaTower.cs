using UnityEngine;

public class FireNovaTower : Tower
{
    public override TowerType Type => TowerType.FireNova;
    private int numProjectiles = 18;
    private float offset;
    protected float damageTickGap;
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override string Name => "Fire Nova";
    public override Faction Faction => Faction.Fire;
    public override ResourceDAO PowerUpCost
    {
        get
        {
            return new ResourceDAO(
                gold: 100 * this.Tier,
                wood: 40 * this.Tier,
                stone: 10 * this.Tier);
        }
    }
    protected override InputController inputController
    {
        get
        {
            if (this._inputController == null)
            {
                this._inputController = new VectorAutoInput(this);
            }
            return this._inputController;
        }
    }

    protected override GameObject CreateProjectile(InputDAO input)
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
                Mathf.Sin(Mathf.Deg2Rad * (360f / (float)numProjectiles) * (float)i)) * (float)Stats.ProjectileMovementSpeed;
            SetProjectileValues(instProj, input);
        }
        return null;
    }

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        p.GetComponent<IConstantDamageProjectile>().SetParameters(
            Stats.Damage,
            Stats.ProjectileLifespan,
            this,
            this.damageTickGap);
    }

    public override TowerStats GetTowerParameters(int level)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100;
        stats.Damage = 10;
        stats.Inaccuracy = .1f;
        stats.ProjectileMovementSpeed = .7f;
        stats.FireCooldown = 4f;
        stats.ProjectileLifespan = 1.5f;
        this.damageTickGap = .5f;
        stats.Range = 1.5f;
        return stats;
    }
}