using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public int pierce;

    public Builder builder;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBuilder(Builder builder)
    {
        this.builder = builder;
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
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        this.damage = 0;
    }
}
