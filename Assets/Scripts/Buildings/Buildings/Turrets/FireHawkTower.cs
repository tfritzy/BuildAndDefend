using UnityEngine;

public class FireHawkTower : TargetLocationFlyingProjTower
{
    public override BuildingType Type => BuildingType.FireHawks;
    public override string Name => "Fire Hawk";
    public override Faction Faction => Faction.Fire;
    public override Vector2Int Size => new Vector2Int(1, 1);
    protected override InputController inputController
    {
        get
        {
            if (_inputController == null)
            {
                _inputController = new TargetObjectAutoInput(this);
            }
            return _inputController;
        }
    }
    public override ResourceDAO PowerUpCost
    {
        get
        {
            return new ResourceDAO(
                gold: 100 * this.Tier,
                wood: 40 * this.Tier,
                stone: 10 * this.Tier
            );
        }
    }

    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + 10 * level + 5 * tier;
        stats.Damage = 10 + level * 5 + tier * 3;
        stats.Inaccuracy = .1f;
        stats.ProjectileMovementSpeed = 3f;
        stats.FireCooldown = .3f;
        stats.ProjectileLifespan = 10f;
        stats.ExplosionRadius = 1f;
        return stats;
    }
}