using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private float lastFireTime;
    private Builder builder;

    public float fireCooldown;
    public GameObject projectile;
    public int projectilePierce;
    public float projectileSpeed;
    public int projectileDamage = 5;

	// Use this for initialization
	void Start () {
        this.builder = GameObject.Find("BuildModeButton").GetComponent<Builder>();
	}
	
	// Update is called once per frame
	void Update () {
        Fire();
    }

    private void Fire()
    {
        if (builder.inBuildMode)
            return;
        if (Time.time < fireCooldown + lastFireTime)
        {
            return;
        }

        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = Camera.main.ScreenToWorldPoint(location);

            Vector2 fireDirection = location - (Vector2)this.transform.position;
            fireDirection = fireDirection / fireDirection.magnitude;

            GameObject instProj = Instantiate(this.projectile, this.transform.position, new Quaternion());
            instProj.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
            instProj.SendMessage("SetDamage", this.projectileDamage);
            instProj.SendMessage("SetPierce", this.projectilePierce);
            instProj.SendMessage("SetBuilder", this.builder);
            lastFireTime = Time.time;
        }
    }
}
