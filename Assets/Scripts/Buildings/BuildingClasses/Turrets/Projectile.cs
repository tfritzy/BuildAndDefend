using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public int pierce;

    // TODO: Fix this garbage
    public Builder builder;
    public float lifespan;

    protected float creationTime;

    void Update(){
        CheckLifespan();
    }

    void Start(){
        this.creationTime = Time.time;
    }

    protected void CheckLifespan(){
        if (Time.time > creationTime + lifespan){
            Destroy(this.gameObject);
        }
    }

    public void SetBuilder(Builder builder)
    {
        this.builder = builder;
    }

    public void SetLifespan(float lifespan){
        this.lifespan = lifespan;
    }

    private void SetDamage(int amount)
    {
        this.damage = amount;
    }

    private void SetPierce(int count)
    {
        this.pierce = count;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Brush")
        {
            builder.AddWood(damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Wall")
            Destroy(this.gameObject);
        else if (collision.gameObject.tag == "Zombie")
        {
            collision.gameObject.SendMessage("TakeDamage", damage);
            Destroy(this.gameObject);
        }
        this.damage = 0;
    }
}
