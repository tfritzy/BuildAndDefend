using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private GameObject Structure;
    public abstract ResourceDAO BuildCost { get; }
    protected int startingHealth;

    /// <summary>
    /// The Size of the building where x=Lenth, y=Height. Zero indexed.
    /// </summary>
    /// <value>The size of the building in <Length, Height> </value>
    public abstract Vector2Int Size { get; }
    public abstract string Name { get; }
    public abstract BuildingType Type { get; }
    public abstract PathableType PathableType { get; }
    public virtual bool StopsProjectiles => false;
    public abstract Faction Faction { get; }
    public abstract ResourceDAO PowerUpCost { get; }
    private ResourceDAO _levelUpCost = new ResourceDAO(skillPoints: 1);
    public virtual ResourceDAO LevelUpCost { get { return _levelUpCost; } }
    public string BuildingId
    {
        get
        {
            if (_id == null)
            {
                _id = $"{this.Name}_{Guid.NewGuid().ToString("N").Substring(0, 6)}";
            }
            return _id;
        }
        set { _id = value; }
    }
    private string _id;

    public int Level
    {
        get { return Player.PlayerData.Values.BuildingUpgrades[this.Type].Level; }
    }
    public int Tier
    {
        get { return Player.PlayerData.Values.BuildingUpgrades[this.Type].Tier; }
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
        this.Stats = GetStats(this.Level, this.Tier);
    }

    public virtual BuildingStats GetStats(int level, int tier)
    {
        return new BuildingStats(health: 100 + level * 10);
    }

    public BuildingStats GetUpgradeStats(int numLevels, int numTiers)
    {
        return GetStats(this.Level + numLevels, this.Tier + numTiers);
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
        this.Position = Map.WorldPointToGridPoint(this.transform.position);
        this.name = BuildingId;
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

    public Vector2 GetWorldPointFromGridPoint(Vector2Int gridPoint)
    {
        Vector2Int topRight = gridPoint + this.Size;
        Vector2 deltaVector = Map.GridPointToWorldPoint(topRight) - Map.GridPointToWorldPoint(gridPoint);
        deltaVector = deltaVector / 2;
        return Map.GridPointToWorldPoint(gridPoint) + deltaVector;
    }

    protected void updateHealthbar()
    {
        Vector3 scale = this.healthbar.transform.localScale;
        scale.x = ((float)Stats.Health / this.startingHealth) * this.startingHealthbarScale;
        this.healthbar.transform.localScale = scale;
    }

    public BuildingOnMapDAO ToBuildingOnMapDAO()
    {
        return new BuildingOnMapDAO
        {
            BuildingId = this.BuildingId,
            Type = this.Type,
            xPos = this.Position.x,
            yPos = this.Position.y,
        };
    }

    public Vector2Int GetClosestPointOnBuildingToGridPosition(Vector2Int gridPos)
    {
        Vector2Int closestPos = Vector2Int.zero;
        if (gridPos.x >= this.Position.x + this.Size.x)
        {
            closestPos.x = this.Position.x + this.Size.x;
        }
        else if (gridPos.x <= this.Position.x)
        {
            closestPos.x = this.Position.x;
        }
        else
        {
            closestPos.x = gridPos.x;
        }

        if (gridPos.y >= this.Position.y + this.Size.y)
        {
            closestPos.y = this.Position.y + this.Size.y;
        }
        else if (gridPos.y <= this.Position.y)
        {
            closestPos.y = this.Position.y;
        }
        else
        {
            closestPos.y = gridPos.y;
        }
        return closestPos;
    }
}
