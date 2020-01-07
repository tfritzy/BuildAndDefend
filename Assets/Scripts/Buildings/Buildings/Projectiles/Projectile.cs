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
        CheckLifespan();
    }

    void Start()
    {
        this.CreationTime = Time.time;
    }

    public virtual void SetValues(int damage, float lifespan, int pierceCount, Tower owner){
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

    protected virtual bool IsTargetCollision(Collision2D collision)
    {
        if (NumHits > PierceCount){
            return false;
        }
        return (collision.gameObject.CompareTag("Zombie"));
    }

    protected virtual void OnFindTargetCollision(GameObject target)
    {
        NumHits += 1;
        List<GameObject> damageTakers = GetDamageTakers(target);
        DealDamage(damageTakers);
        OnHalt(target);
    }

    protected virtual List<GameObject> GetDamageTakers(GameObject initialCollision){
        return new List<GameObject> { initialCollision };
    }

    protected virtual void DealDamage(List<GameObject> damageTakers){
        foreach (GameObject damageTaker in damageTakers){
            damageTaker.GetComponent<Zombie>().TakeDamage(this.Damage, this.Owner);
            NumHits += 1;
        }
    }

    protected virtual void DestroyThis(){
        Destroy(this.gameObject);
    }

    protected virtual bool IsHaltingObject(Collision2D collision)
    {
        return (collision.gameObject.CompareTag("Brush"));
    }

    protected virtual void OnHalt(GameObject haltingObject)
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsHaltingObject(collision))
        {
            OnHalt(collision.gameObject);
        }
        else if (IsTargetCollision(collision))
        {
            OnFindTargetCollision(collision.gameObject);
        }
    }
}
