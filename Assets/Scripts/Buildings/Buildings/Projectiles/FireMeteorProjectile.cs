using System.Collections.Generic;
using UnityEngine;

public class FireMeteorProjectile : TargetLocationFlyingProjectile
{
    protected override BuildingType TowerType => BuildingType.FireMeteor;

    protected override bool ConstantVelocity => true;

    public float startSize = 20f;
    public float endSize = 6f;
    public float startZ = -200;

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        // float currentSize = ((startSize - endSize) / startZ) * this.transform.position.z + startSize;
        // this.transform.localScale = new Vector2(currentSize, currentSize);
    }

    private float GenerateStartingPosition()
    {
        if (Random.Range(0, 2) == 1)
        {
            return Random.Range(-50f, -30f);
        }
        else
        {
            return Random.Range(30f, 50f);
        }
    }

    protected override void Startup()
    {
        base.Startup();
        this.transform.position = new Vector3(
            Random.Range(GenerateStartingPosition(), GenerateStartingPosition()),
            Random.Range(GenerateStartingPosition(), GenerateStartingPosition()),
            startZ);
        Vector3 diffVector = this.TargetPosition - this.transform.position;
        diffVector /= diffVector.magnitude;
        diffVector *= this.MovementSpeed;
        Vector3 initialVelocity = diffVector;
        this.gameObject.GetComponent<Rigidbody>().velocity = initialVelocity;
        Destroy(this.gameObject.GetComponent<Collider2D>());
    }
}
