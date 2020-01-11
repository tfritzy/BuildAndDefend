using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ConstantDamageProjectile : Projectile, IConstantDamageProjectile
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

    public float DamageTickGapInSeconds { get; set; }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        Tower owner,
        float damageTickGapInSeconds)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.PierceCount = int.MaxValue;
        this.Owner = owner;
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