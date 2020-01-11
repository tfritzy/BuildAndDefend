using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveProjectile : Projectile
{
    public float ExplosionRadius;

    [Obsolete("The other set parameters needs to be used for this class.", true)]
    public override void SetParameters(int damage, float lifespan, int pierceCount, Tower owner)
    {
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius
    )
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.PierceCount = pierceCount;
        this.Owner = owner;
        this.ExplosionRadius = explosionRadius;
    }

    protected override void OnHalt(GameObject haltingObject)
    {
        List<GameObject> hits = GetExplosionHits(haltingObject);
        DealDamage(hits);
        GameObject explosion = GetExplosionGameObject();
        explosion.transform.position = this.transform.position;
        base.OnHalt(haltingObject);
    }

    protected GameObject GetExplosionGameObject()
    {
        return Instantiate(Resources.Load<GameObject>($"{FilePaths.Projectiles}/Explosions/{this.TowerType}Explosion"));
    }

    protected virtual List<GameObject> GetExplosionHits(GameObject initialCollision)
    {
        List<GameObject> hits = new List<GameObject>();
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(initialCollision.transform.position, ExplosionRadius);
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
