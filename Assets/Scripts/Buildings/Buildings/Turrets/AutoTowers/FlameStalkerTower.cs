using System;
using UnityEngine;

public class FlameStalkerTower : Tower
{
    public override TowerType Type => TowerType.FlameStalker;

    protected override InputController inputController
    {
        get
        {
            if (this._inputController == null)
            {
                this._inputController = new TargetObjectAutoInput(this);
            }
            return this._inputController;
        }
    }

    protected override void Fire(InputDAO input)
    {
        if (!(input is TargetInputDAO))
        {
            throw new ArgumentException("Input was not the correct type.");
        }

        CreateProjectile(input);
        lastFireTime = Time.time;
    }

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 30;
        this.inaccuracy = .9f;
        this.ProjectileMovementSpeed = 1f;
        this.FireCooldown = 3f;
        this.ProjectileLifespan = 20f;
        this.Range = 5f;
        this.projectileExplosionRadius = .3f;
    }

    protected override void SetProjectileValues(UnityEngine.GameObject p, InputDAO input)
    {
        p.GetComponent<ITargetEntityFlyingProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius,
            ((TargetObjectAutoInput)this.inputController).input.Target,
            this.ProjectileMovementSpeed
        );
    }
}