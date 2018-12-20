using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public int pierce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        if (collision.gameObject.tag == "Wall")
            Destroy(this.gameObject);
        if (collision.gameObject.tag == "Zombie")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
