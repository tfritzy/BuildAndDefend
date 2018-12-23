using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Start () {
        this.health = 200;
	}
	
	// Update is called once per frame
	void Update () {
		
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
