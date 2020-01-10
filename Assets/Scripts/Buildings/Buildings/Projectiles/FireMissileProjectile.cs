using System.Collections.Generic;
using UnityEngine;

public class FireMissileProjectile : TargetLocationProjectile
{
    private float projectileMoveForce = 1f;
    protected override string explosionPrefabName => "FireMissileExplosion";
    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        Vector3 forceVector = this.TargetPosition - this.transform.position;
        forceVector /= forceVector.magnitude;
        forceVector *= projectileMoveForce;
        this.GetComponent<Rigidbody2D>().AddForce(forceVector);
    }

    protected override void Startup()
    {
        Vector3 initialVelocity = new Vector3(Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10));
        this.gameObject.GetComponent<Rigidbody2D>().velocity = initialVelocity;
        this.gameObject.GetComponent<Rigidbody2D>().mass = 1f;
    }
}
