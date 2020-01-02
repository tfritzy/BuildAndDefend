using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    public float fireCooldown;
    public GameObject projectile;
    public int projectilePierce;
    public virtual float projectileSpeed { get; set; }
    public virtual int projectileDamage { get; set; }
    public virtual float projectileLifespan { get; set; }

    public virtual BuildingType Type { get; }

    protected float inaccuracy;
    protected Builder builder;

    private float lastFireTime;

    public override string StructPath { get => ""; }
    public override int WoodCost { get => 200; }

    protected virtual void SetParameters()
    {
        this.Health = 100;
        this.projectileDamage = 5;
        this.inaccuracy = .05f;
        this.projectileSpeed = 10;
        this.fireCooldown = 0.1f;
        this.projectileLifespan = 1f;
    }

    protected override void Setup()
    {
        // TODO: Fix this garbage
        this.builder = GameObject.Find("Builder").GetComponent<Builder>();
        SetParameters();
    }

    // Update is called once per frame
    void Update()
    {
        if (builder.inBuildMode)
            return;
        Fire();
    }

    protected virtual void Fire()
    {
        if (Time.time < fireCooldown + lastFireTime)
        {
            return;
        }

        Vector2? inputLocation = GetInputLocation();
        if (inputLocation.HasValue)
        {
            Vector2 fireDirection = CalculateProjectileTargetLocation(inputLocation.Value);
            CreateProjectile(fireDirection);

            lastFireTime = Time.time;
        }
    }

    protected Vector2? GetInputLocation()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            // TODO: Check what will happen if click on 0,0,0
            Vector2 location = Input.mousePosition != Vector3.zero
                ? (Vector2)Input.mousePosition
                : Input.GetTouch(0).position;
            location = Camera.main.ScreenToWorldPoint(location);
            return location;
        }
        else
        {
            return null;
        }
    }

    protected Vector2 CalculateProjectileTargetLocation(Vector2 touchLocation)
    {
        Vector2 fireDirection = touchLocation - (Vector2)this.transform.position;
        fireDirection = fireDirection / fireDirection.magnitude;

        Vector2 shakeAmount = new Vector3(
            Random.Range(-1 * inaccuracy, 1 * inaccuracy),
            Random.Range(-1 * inaccuracy, 1 * inaccuracy));
        fireDirection += shakeAmount;

        return fireDirection;
    }

    protected void CreateProjectile(Vector2 fireDirection)
    {
        GameObject instProj = Instantiate(
            this.projectile,
            this.transform.position,
            new Quaternion());
        instProj.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
        instProj.SendMessage("SetDamage", this.projectileDamage);
        instProj.SendMessage("SetPierce", this.projectilePierce);
        instProj.SendMessage("SetBuilder", this.builder);
        instProj.SendMessage("SetLifespan", this.projectileLifespan);
    }

    protected override void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
