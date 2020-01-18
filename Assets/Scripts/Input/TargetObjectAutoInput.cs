using UnityEngine;

public class TargetObjectAutoInput : VectorAutoInput
{
    private TargetInputDAO _input;
    public TargetInputDAO input
    {
        get
        {
            if (_input == null)
            {
                _input = new TargetInputDAO();
            }
            return _input;
        }
        set
        {
            _input = value;
        }
    }

    public TargetObjectAutoInput(Tower tower) : base(tower)
    {

    }

    public override InputDAO GetInput()
    {
        if (this.Target != null)
        {
            if (Vector3.Distance(Tower.gameObject.transform.position, Target.transform.position) > Tower.Range)
            {
                this.Target = null;
                this.input.Target = null;
            }
            else
            {
                this.input.Target = Target;
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
}