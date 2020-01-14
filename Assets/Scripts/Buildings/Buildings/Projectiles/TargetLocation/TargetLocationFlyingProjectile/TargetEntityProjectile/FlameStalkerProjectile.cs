public class FlameStalkerProjectile : TargetEntityFlyingProjectile
{
    protected override TowerType TowerType => TowerType.FlameStalker;

    protected override bool ConstantVelocity => true;
}