using UnityEngine;

public class FlamePlumeTower : TargetLocationTower
{
    public override BuildingType Type => BuildingType.FlamePlume;
    public override string Name => "Flame Plume";
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

    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + 10 * level + 5 * tier;
        stats.Damage = 10 + level * 5 + tier * 3;
        stats.Inaccuracy = .1f;
        stats.ProjectileMovementSpeed = 5;
        stats.FireCooldown = .3f;
        stats.ProjectileLifespan = 20f;
        stats.ExplosionRadius = 1f;
        this.explosionDelay = 1f;
        return stats;
    }

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        p.GetComponent<ITimedExplosionProjectile>().SetParameters(
            Stats.Damage,
            Stats.ProjectileLifespan,
            Stats.Pierce,
            this,
            Stats.ExplosionRadius,
            this.explosionDelay
        );
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

    public override void Setup()
    {
        base.Setup();
    }
}