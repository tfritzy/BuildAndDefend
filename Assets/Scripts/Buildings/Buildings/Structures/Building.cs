using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private GameObject Structure;
    public abstract ResourceDAO BuildCost { get; }
    protected int startingHealth;
    public abstract bool IsTower { get; }

    /// <summary>
    /// The Size of the building where x=Lenth, y=Height. Zero indexed.
    /// </summary>
    /// <value>The size of the building in <Length, Height> </value>
    public abstract Vector2Int Size { get; }
    public abstract string Name { get; }
    public abstract TowerType Type { get; }
    public abstract PathableType PathableType { get; }
    public virtual bool StopsProjectiles => false;
    public abstract Faction Faction { get; }
    public abstract ResourceDAO PowerUpCost { get; }
    private ResourceDAO _levelUpCost = new ResourceDAO(skillPoints: 1);
    public virtual ResourceDAO LevelUpCost { get { return _levelUpCost; } }
    public int Level
    {
        get { return Player.Data.vals.BuildingUpgrades[this.Type].Level; }
        set { Player.Data.vals.BuildingUpgrades[this.Type].Level = value; }
    }
    public int Tier
    {
        get { return Player.Data.vals.BuildingUpgrades[this.Type].Tier; }
        set { Player.Data.vals.BuildingUpgrades[this.Type].Level = value; }
    }
    public BuildingStats Stats;

    /// <summary>
    /// The (0,0) position of this building. It may occupy more spots as determined by Building.Size
    /// </summary>
    public Vector2Int Position;
    void Start()
    {
        Setup();
    }

    public virtual void SetStats()
    {
        this.Stats = GetStats(this.Level);
    }

    public virtual BuildingStats GetStats(int level)
    {
        return new BuildingStats(health: 100 + level * 10);
    }

    public BuildingStats GetNextLevelStats()
    {
        return GetStats(this.Level + 1);
    }

    public void TakeDamage(int amount)
    {
        Stats.Health -= amount;
        updateHealthbar();
        if (Stats.Health <= 0)
        {
            Stats.Health = 0;
            Delete();
        }
    }

    protected virtual void OnDeath() { }
    public virtual void Setup()
    {
        SetupHealthbar();
        this.startingHealth = Stats.Health;
    }

    private bool hasAlreadyBeenDeleted = false;
    public void Delete()
    {
        if (hasAlreadyBeenDeleted)
        {
            return;
        }
        hasAlreadyBeenDeleted = true;
        OnDeath();
        Map.RemoveBuildingFromMap(this);
        Destroy(this.gameObject);
    }

    protected GameObject healthbar;
    protected float startingHealthbarScale;
    protected string healthbarPrefabName => "buildingHealthbar";
    private void SetupHealthbar()
    {
        this.healthbar = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Buildings}/{healthbarPrefabName}"),
            this.transform.position + new Vector3(-.4f, .25f),
            new Quaternion(),
            this.transform);
        healthbar.name = "healthbar";
        this.startingHealthbarScale = this.healthbar.transform.localScale.y;
    }

    protected void updateHealthbar()
    {
        Vector3 scale = this.healthbar.transform.localScale;
        scale.x = ((float)Stats.Health / this.startingHealth) * this.startingHealthbarScale;
        this.healthbar.transform.localScale = scale;
    }
}
