using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AutoAttackTower : Tower
{
    public float Range;
    public GameObject Target;
    public override Vector2Int Size => new Vector2Int(0, 0);

    protected float lastFindTargetTime;
    protected float minTimeBetweenTargetFinds = .5f;
    protected override void UpdateLoop()
    {
        if (this.Target == null)
        {
            if (Time.time > lastFindTargetTime + minTimeBetweenTargetFinds)
            {
                this.Target = FindTarget();
                this.lastFindTargetTime = Time.time;
            }
        }
        else
        {
            if (CanFire())
            {
                Fire();
            }
        }
    }

    protected override bool CanFire()
    {
        return (Time.time > FireCooldown + lastFireTime);
    }

    protected virtual GameObject FindTarget()
    {
        GameObject closestZombie = FindClosestZombieWithinRange();
        if (closestZombie != null)
        {
            return closestZombie;
        }
        return null;
    }

    protected GameObject FindClosestZombieWithinRange()
    {
        Collider2D[] zombiesWithinRange = Physics2D.OverlapCircleAll(this.transform.position, this.Range).Where(col => col.gameObject.CompareTag(Tags.Zombie)).ToArray();
        float closestDist = float.MaxValue;
        GameObject closestZombie = null;
        foreach (Collider2D zombie in zombiesWithinRange)
        {
            float distance = Vector3.Distance(zombie.transform.position, this.transform.position);
            if (distance < closestDist)
            {
                closestDist = distance;
                closestZombie = zombie.gameObject;
            }
        }
        return closestZombie;
    }

    protected override void Fire()
    {
        Vector2 fireDirection = CalculateProjectileTargetLocation(this.Target.transform.position);
        CreateProjectile(fireDirection);
        lastFireTime = Time.time;
    }
}