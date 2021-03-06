﻿using System;
using UnityEngine;

public abstract class Tower : Building
{
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 175, gold: 50, stone: 30); }
    public override PathableType PathableType => PathableType.UnPathable;
    protected virtual string projectilePrefabName => this.Type.ToString();
    public float lastFireTime;
    public new TowerStats Stats;

    // Projectiles that stretch themselves until they hit the nearest object.
    public abstract TowerStats GetTowerParameters(int level, int tier);
    public override void Setup()
    {
        SetStats();
        ConfigureUI();
        base.Setup();
    }

    public override void SetStats()
    {
        this.Stats = (TowerStats)GetStats(this.Level, this.Tier);
    }

    public override BuildingStats GetStats(int level, int tier)
    {
        return GetTowerParameters(level, tier);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLoop();
    }

    protected virtual void UpdateLoop()
    {
        InputLoop();
    }

    protected virtual void ConfigureUI()
    {
        TowerSelectManager.AddTowerButton(this.Type);
    }

    protected virtual void Fire(InputDAO input)
    {
        CreateProjectile(input);
        lastFireTime = Time.time;
    }

    protected virtual bool CanFire()
    {
        return (Time.time > Stats.FireCooldown + lastFireTime);
    }

    protected virtual void InputLoop()
    {
        ProcessInput(GetInput());
    }

    protected virtual InputController inputController
    {
        get
        {
            if (_inputController == null)
            {
                _inputController = new VectorClickInput();
            }
            return _inputController;
        }
    }

    protected InputController _inputController;
    protected InputDAO GetInput()
    {
        return inputController.GetInput();
    }

    protected virtual void ProcessInput(InputDAO input)
    {
        if (input.HasValue() && CanFire())
        {
            Fire(input);
        }
    }

    protected Vector2 CalculateProjectileTargetLocation(Vector2 touchLocation)
    {
        Vector2 fireDirection = touchLocation - (Vector2)this.transform.position;
        fireDirection = fireDirection / fireDirection.magnitude;

        Vector2 shakeAmount = new Vector3(
            UnityEngine.Random.Range(-1 * Stats.Inaccuracy, 1 * Stats.Inaccuracy),
            UnityEngine.Random.Range(-1 * Stats.Inaccuracy, 1 * Stats.Inaccuracy));
        fireDirection += shakeAmount;

        return fireDirection;
    }

    protected void SetProjectileRotation(GameObject projectile, Vector3 fireDirection)
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

    protected virtual GameObject CreateProjectile(InputDAO input)
    {
        Vector2 fireDirection = CalculateProjectileTargetLocation(((VectorInputDAO)input).location.Value);

        // TODO: Have towers pool projectiles
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}"),
            this.transform.position,
            new Quaternion(),
            null);

        SetProjectileRotation(instProj, fireDirection);

        instProj.GetComponent<Rigidbody2D>().velocity = fireDirection * Stats.ProjectileMovementSpeed;
        SetProjectileValues(instProj, input);
        return instProj;
    }

    protected bool isHaltingObject(GameObject gameObject)
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

    protected virtual void SetProjectileValues(GameObject p, InputDAO input)
    {
        ProjectileStatsDAO stats = new ProjectileStatsDAO
        {
            Damage = Stats.Damage,
            Lifespan = Stats.ProjectileLifespan,
            PierceCount = Stats.Pierce,
            Owner = this,
            ExplosionRadius = Stats.ExplosionRadius,
        };
        p.GetComponent<Projectile>().SetParameters(stats);
    }

    protected override void OnDeath()
    {
        LevelManager.ShowLoseScreen();
    }

    public void SetIsActive(bool value)
    {
        this.inputController.IsActive = value;
    }
}
