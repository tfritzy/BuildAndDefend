using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour{

    public GameObject structure;
    public int woodCost;
    public int health;

    void Start()
    {
        this.health = 200;
        Setup();
    }

    public void TakeDamage(int amount)
    {
        this.health -= amount;
        if (this.health <= 0)
        {
            Destroy(this.gameObject);
            this.health = 0;
            GameObject.Find("BuildModeButton").SendMessage("FreeGridLoc", this.transform.position);
            OnDeath();
        }
    }

    protected abstract void OnDeath();
    protected abstract void Setup();
}
