public class FireHawkTower : TargetLocationFlyingProjTower
{
    public override TowerType Type => TowerType.FireHawks;
    public override bool HasExplosiveProjectiles => true;
    protected virtual InputController inputController
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