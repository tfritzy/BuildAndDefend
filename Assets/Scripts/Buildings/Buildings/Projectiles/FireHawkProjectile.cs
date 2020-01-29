using System.Collections.Generic;
using UnityEngine;

public class FireHawkProjectile : TargetLocationFlyingProjectile
{
    public float initialZVelocity = -50f;
    public float initialOpposingSpeed = 10f;
    protected override BuildingType TowerType => BuildingType.FireHawks;

    protected override void Startup()
    {
        base.Startup();
        Vector3 clickDirection = this.targetPosition - this.transform.position;
        clickDirection /= clickDirection.magnitude;
        clickDirection *= initialOpposingSpeed;
        clickDirection = new Vector2(
            -clickDirection.x * Random.Range(.5f, 1.5f),
            -clickDirection.y * Random.Range(.5f, 1.5f)
        );
        Vector3 initialVelocity = new Vector3(
            clickDirection.x,
            clickDirection.y,
            initialZVelocity);
        this.gameObject.GetComponent<Rigidbody>().velocity = initialVelocity;
        this.gameObject.GetComponent<Rigidbody>().drag = 3f;
        this.gameObject.GetComponent<Rigidbody>().angularDrag = 0f;
        Destroy(this.gameObject.GetComponent<Collider2D>());
    }
}
