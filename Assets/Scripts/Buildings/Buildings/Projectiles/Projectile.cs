using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int Damage;
    public int PierceCount;
    public float Lifespan;
    public Tower Owner;
    protected float CreationTime;
    public float MovementSpeed;
    protected abstract BuildingType TowerType { get; }
    protected HashSet<GameObject> hits;
    public float ExplosionRadius;

    void Update()
    {
        UpdateLoop();
    }

    void Start()
    {
        Startup();
    }

    protected virtual void Startup()
    {
        this.CreationTime = Time.time;
        this.hits = new HashSet<GameObject>();
    }

    protected virtual void UpdateLoop()
    {
        CheckLifespan();
    }

    public virtual void SetParameters(int damage, float lifespan, int pierceCount, Tower owner, float explosionRadius = default(float))
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.PierceCount = pierceCount;
        this.Owner = owner;
        this.ExplosionRadius = explosionRadius;
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
        return (collision.gameObject.CompareTag("Zombie"));
    }

    protected virtual void OnTargetCollisionEnter(GameObject target)
    {
        List<GameObject> damageTakers = GetDamageTakers(target);
        DealDamage(damageTakers);
    }

    protected virtual void OnTargetCollisionExit(GameObject target)
    {

    }

    protected virtual List<GameObject> GetDamageTakers(GameObject initialCollision)
    {
        List<GameObject> damageTakers = new List<GameObject>();
        if (initialCollision.GetComponent<Zombie>().health > 0)
        {
            damageTakers.Add(initialCollision);
        }
        return damageTakers;
    }

    protected void DealDamage(List<GameObject> damageTakers)
    {
        foreach (GameObject damageTaker in damageTakers)
        {
            DealDamage(damageTaker);
        }
    }

    protected virtual void DealExplosiveDamage(List<GameObject> damageTakers)
    {
        foreach (GameObject damageTaker in damageTakers)
        {
            damageTaker.GetComponent<Zombie>().TakeDamage(this.Damage, this.Owner);
        }
    }

    protected virtual void DealDamage(GameObject damageTaker)
    {
        if (hits.Contains(damageTaker) || this.hits.Count > PierceCount)
        {
            return;
        }

        this.hits.Add(damageTaker);
        damageTaker.GetComponent<Zombie>().TakeDamage(this.Damage, this.Owner);
        if (this.hits.Count > PierceCount)
        {
            OnHalt(damageTaker);
        }
    }

    protected virtual void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    protected virtual bool IsHaltingObject(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            if (collision.gameObject.GetComponent<EnvironmentTile>().StopsProjectiles)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void OnHalt(GameObject haltingObject)
    {
        if (ExplosionRadius != default(float))
        {
            Explode();
        }
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

    protected virtual void Explode()
    {
        List<GameObject> hits = GetExplosionHits();
        DealExplosiveDamage(hits);
        GameObject explosion = GetExplosionGameObject();
        explosion.transform.position = this.transform.position;
    }

    protected GameObject GetExplosionGameObject()
    {
        return Instantiate(Resources.Load<GameObject>($"{FilePaths.Projectiles}/Explosions/{this.TowerType}Explosion"));
    }

    protected virtual List<GameObject> GetExplosionHits()
    {
        List<GameObject> hits = new List<GameObject>();
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(this.transform.position, this.ExplosionRadius);
        foreach (Collider2D collision in explosionHits)
        {
            if (collision.gameObject.CompareTag(Tags.Zombie))
            {
                hits.Add(collision.gameObject);
            }
        }
        return hits;
    }
}
