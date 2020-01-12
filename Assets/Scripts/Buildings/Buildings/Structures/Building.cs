using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private GameObject Structure;
    public abstract ResourceDAO BuildCost { get; }
    public int Health;
    protected int startingHealth;
    public abstract bool IsTower { get; }

    /// <summary>
    /// The Size of the building where x=Lenth, y=Height. Zero indexed.
    /// </summary>
    /// <value>The size of the building in <Length, Height> </value>
    public abstract Vector2Int Size { get; }

    public abstract TowerType Type { get; }
    public abstract PathableType PathableType { get; }
    public virtual bool StopsProjectiles => false;


    /// <summary>
    /// The (0,0) position of this building. It may occupy more spots as determined by Building.Size
    /// </summary>
    public Vector2Int Position;
    void Start()
    {
        this.Health = 200;
        Setup();
    }

    public void TakeDamage(int amount)
    {
        this.Health -= amount;
        updateHealthbar();
        if (this.Health <= 0)
        {
            this.Health = 0;
            Map.RemoveBuildingFromMap(this);
            OnDeath();
            Delete();
        }
    }

    protected virtual void OnDeath() { }
    protected virtual void Setup()
    {
        this.startingHealth = this.Health;
        SetupHealthbar();
    }

    public void Delete()
    {
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
            this.transform.position + new Vector3(0, .5f),
            new Quaternion(),
            this.transform);
        healthbar.name = "healthbar";
        this.startingHealthbarScale = this.healthbar.transform.localScale.y;
    }

    protected void updateHealthbar()
    {
        Vector3 scale = this.healthbar.transform.localScale;
        scale.y = (this.Health / this.startingHealth) * this.startingHealth;
        this.healthbar.transform.localScale = scale;
    }
}
