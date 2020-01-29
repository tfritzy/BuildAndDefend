using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetFlyingProjectile : Projectile
{
    protected abstract Vector3 targetPosition { get; }
    protected float movementForce;

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        Vector3 diffVector = this.targetPosition - this.transform.position;
        float distance = Vector3.SqrMagnitude(this.targetPosition - this.transform.position);
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
        if (this.movementForce == default)
        {
            diffVector *= MovementSpeed;
            SetVelocity(diffVector);
        }
        else
        {
            diffVector *= movementForce;
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

    protected override void OnTargetCollisionEnter(GameObject target)
    {
        // Do Nothing.
    }

    protected override void OnTargetCollisionExit(GameObject target)
    {
        // Do Nothing.
    }
}
