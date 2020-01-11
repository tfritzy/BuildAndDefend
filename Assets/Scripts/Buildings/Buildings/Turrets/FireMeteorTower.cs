public class FireMeteorTower : TargetLocationFlyingProjTower
{
    public override TowerType Type => TowerType.FireMeteor;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.projectileDamage = 10;
        this.inaccuracy = .1f;
        this.projectileSpeed = 5;
        this.fireCooldown = .3f;
        this.projectileLifespan = 10f;
        this.explosionRadius = 1f;
        this.projectileMovementSpeed = 80f;
    }
}