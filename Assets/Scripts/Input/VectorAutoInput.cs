using System.Linq;
using UnityEngine;

public class VectorAutoInput : InputController
{
    public Tower Tower;

    /// <summary>
    /// Auto towers cannot become inactive.
    /// </summary>
    public override bool IsActive => true;
    private Vector3 lastInputLocation;
    protected GameObject Target;
    private VectorInputDAO _input;
    private VectorInputDAO input
    {
        get
        {
            if (_input == null)
            {
                _input = new VectorInputDAO(null);
            }
            return _input;
        }
        set
        {
            _input = value;
        }
    }

    public VectorAutoInput(Tower tower)
    {
        this.Tower = tower;
        this.IsActive = true;
    }

    protected float lastFindTargetTime;
    protected float minTimeBetweenTargetFinds = .5f;
    public override InputDAO GetInput()
    {
        // shoot the target if it's not null.
        if (this.Target != null)
        {
            if (Vector3.Distance(Tower.gameObject.transform.position, Target.transform.position) > Tower.Stats.Range)
            {
                this.Target = null;
                this.input.location = null;
            }
            else
            {
                this.input.location = this.Target.transform.position;
                lastInputLocation = this.input.location.Value;
            }

            return input;
        }

        // exit if it's too early to check for targets.
        if (Time.time < lastFindTargetTime + minTimeBetweenTargetFinds)
        {
            return input;
        }

        // find the target. if it returns null, no harm done.
        lastFindTargetTime = Time.time;
        this.Target = FindClosestZombieWithinRange();
        return input;
    }

    protected GameObject FindClosestZombieWithinRange()
    {
        Collider2D[] zombiesWithinRange = Physics2D.OverlapCircleAll(Tower.transform.position, Tower.Stats.Range).Where(col => col.gameObject.CompareTag(Tags.Zombie)).ToArray();
        float closestDist = float.MaxValue;
        GameObject closestZombie = null;
        foreach (Collider2D zombie in zombiesWithinRange)
        {
            float distance = Vector3.Distance(zombie.transform.position, Tower.gameObject.transform.position);
            if (distance < closestDist)
            {
                closestDist = distance;
                closestZombie = zombie.gameObject;
            }
        }
        return closestZombie;
    }
}