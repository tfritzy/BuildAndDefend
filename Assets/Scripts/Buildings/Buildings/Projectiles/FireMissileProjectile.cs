using System.Collections.Generic;
using UnityEngine;

public class FireMissileProjectile : TargetLocationFlyingProjectile
{
    private float projectileMoveForce = 50f;
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
        base.Startup();
        Vector3 clickDirection = this.TargetPosition - this.transform.position;
        clickDirection /= clickDirection.magnitude;
        clickDirection *= 5f;
        clickDirection = new Vector2(
            -clickDirection.x * Random.Range(.5f, 1.5f),
            -clickDirection.y * Random.Range(.5f, 1.5f)
        );
        Vector3 initialVelocity = new Vector3(
            clickDirection.x,
            clickDirection.y,
            Random.Range(30, 40));
        this.gameObject.GetComponent<Rigidbody2D>().velocity = initialVelocity;
        this.gameObject.GetComponent<Rigidbody2D>().mass = 5f;
        Destroy(this.gameObject.GetComponent<Collider2D>());
    }
}
