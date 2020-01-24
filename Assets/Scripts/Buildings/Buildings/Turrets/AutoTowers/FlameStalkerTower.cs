using System;
using UnityEngine;

public class FlameStalkerTower : Tower
{
    public override BuildingType Type => BuildingType.FlameStalker;
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override string Name => "Flame Stalker";
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
                this._inputController = new TargetObjectAutoInput(this);
            }
            return this._inputController;
        }
    }

    protected override void Fire(InputDAO input)
    {
        if (!(input is TargetInputDAO))
        {
            throw new ArgumentException("Input was not the correct type.");
        }

        CreateProjectile(input);
        lastFireTime = Time.time;
    }

    protected override GameObject CreateProjectile(InputDAO input)
    {
        // TODO: Have towers pool projectiles
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}"),
            this.transform.position,
            new Quaternion(),
            null);

        SetProjectileValues(instProj, input);
        return instProj;
    }


    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + 10 * level + 5 * tier;
        stats.Damage = 30 + level * 5 + tier * 3;
        stats.Inaccuracy = .9f;
        stats.ProjectileMovementSpeed = 3f;
        stats.FireCooldown = 3f;
        stats.ProjectileLifespan = 20f;
        stats.Range = 5f;
        stats.ExplosionRadius = .3f;
        return stats;
    }

    protected override void SetProjectileValues(UnityEngine.GameObject p, InputDAO input)
    {
        p.GetComponent<ITargetEntityFlyingProjectile>().SetParameters(
            Stats.Damage,
            Stats.ProjectileLifespan,
            Stats.Pierce,
            this,
            Stats.ExplosionRadius,
            ((TargetObjectAutoInput)this.inputController).input.Target,
            Stats.ProjectileMovementSpeed
        );
    }
}