using UnityEngine;

public class RapidFireTower : StretchProjectileTower
{
    public override TowerType Type => TowerType.RapidFire;
    public override string Name => "Rapid Fire";
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
    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 10;
        this.inaccuracy = .1f;
        this.ProjectileMovementSpeed = 0;
        this.FireCooldown = .01f;
        this.ProjectileLifespan = .2f;
        this.ProjectilePierce = int.MaxValue;
    }
}