public class FireSparkTower : AutoAttackTower
{
    public override TowerType Type => TowerType.FireSpark;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 5;
        this.inaccuracy = .05f;
        this.ProjectileMovementSpeed = 10;
        this.FireCooldown = 0.1f;
        this.ProjectileLifespan = 1f;
        this.Range = 1f;
    }
}