using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private GameObject Structure;
    public abstract ResourceDAO BuildCost { get; }
    public int Health;
    public abstract bool IsTower { get; }

    /// <summary>
    /// The Size of the building where x=Lenth, y=Height. Zero indexed.
    /// </summary>
    /// <value>The size of the building in <Length, Height> </value>
    public abstract Vector2Int Size { get; }

    public abstract BuildingType Type { get; }
    public abstract PathableType PathableType { get; }

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
        if (this.Health <= 0)
        {
            this.Health = 0;
            Map.RemoveBuildingFromMap(this);
            OnDeath();
            Delete();
        }
    }

    protected virtual void OnDeath() { }
    protected virtual void Setup() { }

    public void Delete()
    {
        OnDeath();
        Map.RemoveBuildingFromMap(this);
        Destroy(this.gameObject);
    }
}
