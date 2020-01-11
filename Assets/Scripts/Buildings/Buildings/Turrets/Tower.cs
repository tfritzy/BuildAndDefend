using UnityEngine;

public abstract class Tower : Building
{
    public float fireCooldown;
    public int projectilePierce;
    public virtual float projectileSpeed { get; set; }
    public virtual int projectileDamage { get; set; }
    public virtual float projectileLifespan { get; set; }
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 175, gold: 50, stone: 30); }
    public override Vector2Int Size => new Vector2Int(1, 1);
    public override PathableType PathableType => PathableType.UnPathable;
    protected virtual string projectilePrefabName => this.Type.ToString();
    public override bool IsTower => true;
    public bool IsBeingControlled;
    protected float inaccuracy;
    protected float lastFireTime;

    public virtual void SetTowerParameters()
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
        return (Time.time > fireCooldown + lastFireTime);
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

    protected virtual void CreateProjectile(Vector2 fireDirection)
    {
        // TODO: Have towers pool projectiles
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}"),
            this.transform.position,
            new Quaternion(),
            null);
        instProj.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
        Projectile p = instProj.GetComponent<Projectile>();
        SetProjectileValues(p);
    }

    protected virtual void SetProjectileValues(Projectile p)
    {
        p.SetParameters(this.projectileDamage, this.projectileLifespan, this.projectilePierce, this);
    }

    protected override void OnDeath()
    {
        LevelManager.ShowLoseScreen();
    }
}
