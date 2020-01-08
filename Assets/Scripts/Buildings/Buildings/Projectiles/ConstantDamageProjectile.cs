using System.Collections.Generic;
using UnityEngine;
using System;

public class ConstantDamageProjectile : Projectile {

    /// <summary>
    /// Dictionary that stores the enemies currently contained within this entity's realm
    /// </summary>
    /// <typeparam name="GameObject">The gameobject inside the bounds</typeparam>
    /// <typeparam name="float">The last time a damage tick was applied to the enemy</typeparam>
    /// <returns></returns>
    protected Dictionary<GameObject, float> CurrentlyContainedEnemies = new Dictionary<GameObject, float>();
    public float DamageTickGapInSeconds;

    public override void SetParameters(int damage, float lifespan, int pierceCount, Tower owner){
        throw new NotSupportedException("The Set parameters on constant damage projectiles should be called "
         + "With damageTickGapInSecondsParameter");
    }

    public void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float damageTickGapInSeconds)
    {
        base.SetParameters(damage, lifespan, pierceCount, owner);
        this.DamageTickGapInSeconds = damageTickGapInSeconds;
    }

    protected override void UpdateLoop() {
        base.UpdateLoop();
        foreach (GameObject enemy in CurrentlyContainedEnemies.Keys){
            if (Time.time > CurrentlyContainedEnemies[enemy] + DamageTickGapInSeconds){
                CurrentlyContainedEnemies[enemy] = Time.time;
                DealDamage(enemy);
            }
        }
    }

    protected override void OnTargetCollisionEnter(GameObject target) {
        CurrentlyContainedEnemies.Add(target.gameObject, Time.time);
        DealDamage(target.gameObject);
    }

    protected override void OnTargetCollisionExit(GameObject target) {
        CurrentlyContainedEnemies.Remove(target);
    }

}