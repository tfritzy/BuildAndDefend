using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int Damage;
    public int PierceCount;
    public float Lifespan;
    public Tower Owner;
    public int NumHits;
    protected float CreationTime;

    void Update()
    {
        UpdateLoop();
    }

    void Start()
    {
        this.CreationTime = Time.time;
    }

    protected virtual void UpdateLoop()
    {
        CheckLifespan();
    }

    public virtual void SetParameters(int damage, float lifespan, int pierceCount, Tower owner)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.PierceCount = pierceCount;
        this.Owner = owner;
    }

    protected void CheckLifespan()
    {
        if (Time.time > CreationTime + Lifespan)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual bool IsTargetCollision(Collider2D collision)
    {
        if (NumHits > PierceCount)
        {
            return false;
        }
        return (collision.gameObject.CompareTag("Zombie"));
    }

    protected virtual void OnTargetCollisionEnter(GameObject target)
    {
        if (NumHits > PierceCount)
        {
            return;
        }
        NumHits += 1;
        List<GameObject> damageTakers = GetDamageTakers(target);
        DealDamage(damageTakers);
        OnHalt(target);
    }

    protected virtual void OnTargetCollisionExit(GameObject target)
    {

    }

    protected virtual List<GameObject> GetDamageTakers(GameObject initialCollision)
    {
        return new List<GameObject> { initialCollision };
    }

    protected void DealDamage(List<GameObject> damageTakers)
    {
        foreach (GameObject damageTaker in damageTakers)
        {
            DealDamage(damageTaker);
        }
    }

    protected virtual void DealDamage(GameObject damageTaker)
    {
        damageTaker.GetComponent<Zombie>().TakeDamage(this.Damage, this.Owner);
    }

    protected virtual void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    protected virtual bool IsHaltingObject(Collider2D collision)
    {
        return (collision.gameObject.CompareTag("Brush"));
    }

    protected virtual void OnHalt(GameObject haltingObject)
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsHaltingObject(col))
        {
            OnHalt(col.gameObject);
        }
        else if (IsTargetCollision(col))
        {
            OnTargetCollisionEnter(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (IsTargetCollision(col))
        {
            OnTargetCollisionExit(col.gameObject);
        }
    }
}
