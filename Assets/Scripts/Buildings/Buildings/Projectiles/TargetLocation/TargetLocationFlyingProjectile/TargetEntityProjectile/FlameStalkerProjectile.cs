public class FlameStalkerProjectile : TargetEntityFlyingProjectile
{
    protected override BuildingType TowerType => BuildingType.FlameStalker;

    protected override bool ConstantVelocity => true;
}