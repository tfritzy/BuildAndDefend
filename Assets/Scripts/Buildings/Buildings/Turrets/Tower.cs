using UnityEngine;

public abstract class Tower : Building
{
    public float FireCooldown;
    public int ProjectilePierce;
    public virtual float ProjectileMovementSpeed { get; set; }
    public virtual int ProjectileDamage { get; set; }
    public virtual float ProjectileLifespan { get; set; }
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 175, gold: 50, stone: 30); }
    public override Vector2Int Size => new Vector2Int(1, 1);
    public override PathableType PathableType => PathableType.UnPathable;
    public virtual bool HasExplosiveProjectiles => false;
    public float projectileExplosionRadius;
    public bool IsBeingControlled;
    protected virtual string projectilePrefabName => this.Type.ToString();
    public override bool IsTower => true;
    protected float inaccuracy;
    protected float lastFireTime;

    // Projectiles that stretch themselves until they hit the nearest object.
    protected virtual bool hasScalingProjectiles => false;

    public virtual void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 5;
        this.inaccuracy = .05f;
        this.ProjectileMovementSpeed = 10;
        this.FireCooldown = 0.1f;
        this.ProjectileLifespan = 1f;
    }

    protected override void Setup()
    {
        base.Setup();
        SetTowerParameters();
        ConfigureUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanFire())
        {
            Fire();
        }
    }

    protected virtual void ConfigureUI()
    {
        TowerSelectManager.AddTowerButton(this.Type);
    }

    protected virtual void Fire()
    {
        Vector2? inputLocation = GetInputLocation();
        if (inputLocation.HasValue)
        {
            Vector2 fireDirection = CalculateProjectileTargetLocation(inputLocation.Value);
            CreateProjectile(fireDirection);

            lastFireTime = Time.time;
        }
    }

    protected bool CanFire()
    {
        if (!IsBeingControlled)
        {
            return false;
        }
        return (Time.time > FireCooldown + lastFireTime);
    }

    protected Vector2? GetInputLocation()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            // TODO: Check what will happen if click on 0,0,0
            Vector2 location = Input.mousePosition != Vector3.zero
                ? (Vector2)Input.mousePosition
                : Input.GetTouch(0).position;
            location = GameObjectCache.Camera.ScreenToWorldPoint(location);
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
            UnityEngine.Random.Range(-1 * inaccuracy, 1 * inaccuracy),
            UnityEngine.Random.Range(-1 * inaccuracy, 1 * inaccuracy));
        fireDirection += shakeAmount;

        return fireDirection;
    }

    private void ScaleProjectile(GameObject projectile, Vector3 fireDirection)
    {
        ColliderDistance2D projectileLength = closestHaltingHit(fireDirection);
        projectileLength.distance += (projectileLength.pointB - (Vector2)this.transform.position).magnitude;
        Vector3 currentScale = projectile.transform.localScale;
        currentScale.y = currentScale.y * projectileLength.distance;
        projectile.transform.localScale = currentScale;
    }

    private void SetProjectileRotation(GameObject projectile, Vector3 fireDirection)
    {
        float degAngle = Mathf.Rad2Deg * Mathf.Atan(fireDirection.y / fireDirection.x);
        if (fireDirection.x > 0)
        {
            degAngle -= 90;
        }
        else
        {
            degAngle += 90;
        }
        projectile.transform.eulerAngles = new Vector3(0, 0, degAngle);
    }

    protected virtual void CreateProjectile(Vector2 fireDirection)
    {
        // TODO: Have towers pool projectiles
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}"),
            this.transform.position,
            new Quaternion(),
            null);

        SetProjectileRotation(instProj, fireDirection);
        if (hasScalingProjectiles)
        {
            ScaleProjectile(instProj, fireDirection);
        }

        instProj.GetComponent<Rigidbody2D>().velocity = fireDirection * ProjectileMovementSpeed;
        SetProjectileValues(instProj);
    }

    private ColliderDistance2D closestHaltingHit(Vector2 fireDirection)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, fireDirection);
        ColliderDistance2D closestDist = new ColliderDistance2D();
        closestDist.distance = 100f;
        foreach (RaycastHit2D hit in hits)
        {
            if (!isHaltingObject(hit.transform.gameObject))
            {
                continue;
            }

            ColliderDistance2D distance = hit.transform.gameObject.GetComponent<Collider2D>().Distance(this.GetComponent<Collider2D>());
            if (distance.distance < closestDist.distance)
            {
                closestDist = distance;
            }
        }
        return closestDist;
    }

    private bool isHaltingObject(GameObject gameObject)
    {
        if (gameObject.CompareTag("Terrain") && gameObject.GetComponent<EnvironmentTile>().StopsProjectiles)
        {
            return true;
        }
        if (gameObject.GetComponent<Building>() != null && gameObject.GetComponent<Building>().StopsProjectiles)
        {
            return true;
        }
        return false;
    }

    protected virtual void SetProjectileValues(GameObject p)
    {
        p.GetComponent<Projectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius);
    }

    protected override void OnDeath()
    {
        LevelManager.ShowLoseScreen();
    }
}
