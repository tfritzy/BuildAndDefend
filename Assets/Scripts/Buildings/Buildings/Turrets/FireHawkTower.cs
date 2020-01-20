public class FireHawkTower : TargetLocationFlyingProjTower
{
    public override TowerType Type => TowerType.FireHawks;
    public override bool HasExplosiveProjectiles => true;
    public override string Name => "Fire Hawk";
    public override Faction Faction => Faction.Fire;
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
                gold: 100 * this.Level,
                wood: 40 * this.Level,
                stone: 10 * this.Level);
        }
    }

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 10;
        this.inaccuracy = .1f;
        this.ProjectileMovementSpeed = 3f;
        this.FireCooldown = .3f;
        this.ProjectileLifespan = 10f;
        this.projectileExplosionRadius = 1f;
    }
}