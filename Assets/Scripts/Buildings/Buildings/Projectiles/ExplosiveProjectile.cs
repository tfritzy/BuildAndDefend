using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveProjectile : Projectile
{
    public abstract float ExplosionRadius { get; set; }
    protected abstract string explosionPrefabName { get; set; }

    protected override void OnHalt(GameObject haltingObject){
        List<GameObject> hits = GetExplosionHits(haltingObject);
        DealDamage(hits);
        GameObject explosion = GetExplosionGameObject();
        explosion.transform.position = haltingObject.transform.position;
        base.OnHalt(haltingObject);
    }

    protected GameObject GetExplosionGameObject(){
        return Instantiate(Resources.Load<GameObject>($"{FilePaths.Projectiles}/{explosionPrefabName}"));
    }

    protected virtual List<GameObject> GetExplosionHits(GameObject initialCollision){
        List<GameObject> hits = new List<GameObject>();
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(initialCollision.transform.position, ExplosionRadius);
        foreach (Collider2D collision in explosionHits){
            if (collision.gameObject.CompareTag(Tags.Zombie)){
                hits.Add(collision.gameObject);
            }
        }
        return hits;
    }
}
