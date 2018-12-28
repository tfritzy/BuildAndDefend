using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour{

    private GameObject structure;
    public int woodCost;
    public int health;
    public string structPath;

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
            this.health = 0;
            GameObject.Find("BuildModeButton").SendMessage("FreeGridLoc", this.transform.position);
            OnDeath();
            Delete();
        }
    }

    protected abstract void OnDeath();
    protected abstract void Setup();

    public void Delete()
    {
        OnDeath();
        Destroy(this.gameObject);
    }

    public GameObject GetStructure()
    {
        if (this.structure == null)
        {
            this.structure = Resources.Load<GameObject>(this.structPath);
        }
        return this.structure;
    }
}
