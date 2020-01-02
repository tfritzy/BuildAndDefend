﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private GameObject Structure;
    public abstract int WoodCost { get; }
    public int Health;
    public abstract string StructPath { get; }

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
            Map.FreeGridLoc(this.transform.position);
            OnDeath();
            Delete();
        }
    }

    protected virtual void OnDeath() { }
    protected virtual void Setup() { }

    public void Delete()
    {
        OnDeath();
        Destroy(this.gameObject);
    }

    public GameObject GetStructure()
    {
        if (this.Structure == null)
        {
            this.Structure = Resources.Load<GameObject>(this.StructPath);
        }
        return this.Structure;
    }
}
