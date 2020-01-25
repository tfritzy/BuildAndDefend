using UnityEngine;

public class FireSparkTower : Tower
{
    public override BuildingType Type => BuildingType.FireSpark;
    public override string Name => "Fire Spark";
    public override Faction Faction => Faction.Fire;
    public override Vector2Int Size => new Vector2Int(0, 0);
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

    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + level * 5 + tier * 3;
        stats.Damage = 5 + level * 5 + tier * 3;
        stats.Inaccuracy = .05f;
        stats.ProjectileMovementSpeed = 10;
        stats.FireCooldown = 0.3f;
        stats.ProjectileLifespan = 1f;
        stats.Range = 1f;
        return stats;
    }
}