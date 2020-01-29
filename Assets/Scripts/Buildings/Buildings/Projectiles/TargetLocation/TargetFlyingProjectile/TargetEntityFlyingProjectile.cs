using UnityEngine;

public abstract class TargetEntityFlyingProjectile : TargetFlyingProjectile
{
    protected Vector3 lastEntityPosition;
    protected GameObject Target { get; set; }
    protected override Vector3 targetPosition
    {
        get
        {
            if (Target != null)
            {
                lastEntityPosition = Target.transform.position;
                return Target.transform.position;
            }
            else
            {
                return lastEntityPosition;
            }
        }
    }

    public override void SetParameters(ProjectileStatsDAO stats)
    {
        if (!(stats is TargetEntityProjectileStatsDAO))
        {
            throw new System.ArgumentException("The projectileStats must be of the type TargetEntityProjectileStats");
        }

        base.SetParameters(stats);
        this.Target = ((TargetEntityProjectileStatsDAO)stats).Target;
        this.MovementSpeed = ((TargetEntityProjectileStatsDAO)stats).MovementSpeed;
    }
}