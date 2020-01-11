using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ConstantDamageProjectile : Projectile
{

    /// <summary>
    /// List that stores the enemies currently contained within this entity's realm
    /// </summary>
    /// <typeparam name="DamageTickTracker">The gameobject inside the bounds along with the last time it was damaged</typeparam>
    /// <returns></returns>
    protected List<DamageTickTracker> CurrentlyContainedEnemies = new List<DamageTickTracker>();


    protected struct DamageTickTracker
    {
        public DamageTickTracker(GameObject enemy, float lastDamageTickTime)
        {
            this.enemy = enemy;
            this.lastDamageTickTime = lastDamageTickTime;
        }
        public GameObject enemy;
        public float lastDamageTickTime;
    }

    public float DamageTickGapInSeconds;

    public override void SetParameters(int damage, float lifespan, int pierceCount, Tower owner)
    {
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

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        for (int i = 0; i < CurrentlyContainedEnemies.Count; i++)
        {
            DamageTickTracker tracker = CurrentlyContainedEnemies[i];
            if (tracker.enemy == null)
            {
                CurrentlyContainedEnemies.RemoveAt(i);
            }
            if (Time.time > tracker.lastDamageTickTime + DamageTickGapInSeconds)
            {
                tracker.lastDamageTickTime = Time.time;
                DealDamage(tracker.enemy);
            }
        }
    }

    protected override void OnTargetCollisionEnter(GameObject target)
    {
        CurrentlyContainedEnemies.Add(new DamageTickTracker(target.gameObject, Time.time));
        DealDamage(target.gameObject);
    }

    protected override void OnTargetCollisionExit(GameObject target)
    {
        for (int i = 0; i < CurrentlyContainedEnemies.Count; i++)
        {
            if (CurrentlyContainedEnemies[i].enemy.Equals(target))
            {
                CurrentlyContainedEnemies.RemoveAt(i);
                return;
            }
        }
    }

}