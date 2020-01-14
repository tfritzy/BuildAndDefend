using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetLocationFlyingProjectile : Projectile, ITargetLocationFlyingProjectile
{
    private Vector3 targetPosition;
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    protected abstract bool ConstantVelocity { get; }
    public float projectileMoveForce = 50;

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        Vector3 diffVector = this.TargetPosition - this.transform.position;
        float distance = Vector3.SqrMagnitude(this.TargetPosition - this.transform.position);
        if (Mathf.Abs(distance) < .1f || this.transform.position.z > 0)
        {
            this.OnHalt(this.gameObject);
        }
        Move(diffVector);
    }

    protected bool has3DRigidbody;
    protected override void Startup()
    {
        base.Startup();
        has3DRigidbody = this.GetComponent<Rigidbody2D>() == null;
    }

    protected virtual void Move(Vector3 diffVector)
    {
        diffVector /= diffVector.magnitude;
        if (ConstantVelocity)
        {
            diffVector *= MovementSpeed;
            SetVelocity(diffVector);
        }
        else
        {
            diffVector *= projectileMoveForce;
            AddForce(diffVector);
        }
    }

    private void AddForce(Vector3 force)
    {
        if (this.has3DRigidbody)
        {
            this.GetComponent<Rigidbody>().AddForce(force);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }

    private void SetVelocity(Vector3 velocity)
    {
        if (this.has3DRigidbody)
        {
            this.GetComponent<Rigidbody>().velocity = velocity;
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        Vector3 targetPosition,
        float movementSpeed)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.Owner = owner;
        this.TargetPosition = targetPosition;
        this.ExplosionRadius = explosionRadius;
        this.PierceCount = pierceCount;
        this.MovementSpeed = movementSpeed;
    }

    protected override void OnTargetCollisionEnter(GameObject target)
    {
        // Do Nothing.
    }

    protected override void OnTargetCollisionExit(GameObject target)
    {
        // Do Nothing.
    }
}
