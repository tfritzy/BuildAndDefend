using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public int damage;
    public int pierce;

    // TODO: Fix this garbage
    public Builder builder;
    public float lifespan;
    public Tower Attacker;

    protected float creationTime;

    void Update()
    {
        CheckLifespan();
    }

    void Start()
    {
        this.creationTime = Time.time;
    }

    protected void CheckLifespan()
    {
        if (Time.time > creationTime + lifespan)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetBuilder(Builder builder)
    {
        this.builder = builder;
    }

    public void SetLifespan(float lifespan)
    {
        this.lifespan = lifespan;
    }

    public void SetDamage(int amount)
    {
        this.damage = amount;
    }

    public void SetPierce(int count)
    {
        this.pierce = count;
    }

    public void SetAttacker(Tower attacker)
    {
        this.Attacker = attacker;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brush"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zombieScript = collision.gameObject.GetComponent<Zombie>();
            if (zombieScript == null)
            {
                throw new System.Exception("Gameobject tagged zombie should have a zombie script.");
            }
            if (Attacker == null)
            {
                throw new System.Exception("Attacker not set for this projectile.");
            }
            zombieScript.TakeDamage(damage, Attacker);
            Destroy(this.gameObject);
        }
        this.damage = 0;
    }
}
