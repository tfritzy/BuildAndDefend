using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public int health;


	void Start () {
        this.health = 200;
	}
	
    public void TakeDamage(int amount)
    {
        this.health -= amount;
        if (this.health <= 0)
        {
            Destroy(this.gameObject);
            this.health = 0;
            GameObject.Find("BuildModeButton").SendMessage("FreeGridLoc", this.transform.position);
        }
    }
}
