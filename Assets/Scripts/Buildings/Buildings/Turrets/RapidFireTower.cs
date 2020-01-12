public class RapidFireTower : Tower
{
    public override TowerType Type => TowerType.RapidFire;
    protected override bool hasScalingProjectiles => true;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 10;
        this.inaccuracy = .1f;
        this.ProjectileMovementSpeed = 0;
        this.FireCooldown = 1f;
        this.ProjectileLifespan = .2f;
        this.ProjectilePierce = int.MaxValue;
    }
}