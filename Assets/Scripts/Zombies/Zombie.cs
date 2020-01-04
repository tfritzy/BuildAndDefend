using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    protected List<Vector2> path;
    protected int[] locationInGrid;
    protected int[] targetLoc;
    protected bool atFinalLoc = false;
    protected int pathProgress = 0;
    protected int damage = 1;
    protected float lastAttackTime;
    protected float attackSpeed = 1;
    protected bool needsNewPath;
    protected float calculateNewPathTime;

    public float zombieSpeed = .3f;
    public int health = 5;
    public GameObject target;
    public virtual ResourceDAO KillReward { get => new ResourceDAO(gold: 10); }


    // Use this for initialization
    void Start()
    {
        this.path = new List<Vector2>();
        lastAttackTime = Time.time;
        RestartPath();
        ChildrenSetup();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
        Attack();
    }

    /// <summary>
    /// Let this zombie know that its path was broken and it needs to recalculate.
    /// </summary>
    public void NotifyOfPathBreak()
    {
        this.needsNewPath = true;
        this.calculateNewPathTime = Time.time + Random.Range(.1f, 1f);
    }

    protected void FollowPath()
    {
        if (this.needsNewPath && Time.time > this.calculateNewPathTime)
        {
            RestartPath();
            this.needsNewPath = false;
        }
        if (atFinalLoc)
        {
            if (target == null)
                return;
            this.transform.position = Vector2.MoveTowards(transform.position, target.transform.position, zombieSpeed * Time.deltaTime);
            return;
        }
        if (path == null || path.Count <= 0)
        {
            return;
        }
        this.transform.position = Vector2.MoveTowards(transform.position, path[pathProgress], zombieSpeed * Time.deltaTime);
        if ((path[pathProgress] - (Vector2)transform.position).magnitude < .3f)
        {
            pathProgress += 1;
            if (pathProgress >= path.Count)
            {
                atFinalLoc = true;
            }
        }
    }

    protected virtual void ChildrenSetup()
    {
        return;
    }
    void Attack()
    {
        if (!atFinalLoc)
        {
            return;
        }

        if (Time.time < lastAttackTime + attackSpeed)
        {
            return;
        }

        if (target != null)
        {
            target.SendMessage("TakeDamage", this.damage);
        }
        else
        {
            RestartPath();
        }
        lastAttackTime = Time.time;
    }

    public void SetPath(List<Vector2> newPath)
    {
        pathProgress = 0;
        UnsubscribeToPath();
        this.path = newPath;
        this.needsNewPath = false;
        SubscribeToPath();
    }

    public virtual List<Vector2> RestartPath()
    {
        pathProgress = 0;
        UnsubscribeToPath();
        this.target = GameObject.Find("Turret");
        this.locationInGrid = Map.WorldPointToGridPoint(this.transform.position);
        this.targetLoc = Map.WorldPointToGridPoint(GameObject.Find("Turret").transform.position);
        this.path = FindPath(Map.Grid, locationInGrid, this.targetLoc);

        Collider2D[] nearbyZombs = Physics2D.OverlapCircleAll(this.transform.position, 1f);
        foreach (Collider2D col in nearbyZombs)
        {
            if (col.tag != "Zombie" || col.gameObject == this.gameObject)
                continue;
            if (col == null)
            {
                continue;
            }
            Zombie colZomb = col.GetComponent<Zombie>();
            colZomb.SetPath(this.path);
        }

        SubscribeToPath();
        return path;
    }

    // Perform BFS to find shortest path to the desired location
    public List<Vector2> FindPath(TileType[,] grid, int[] startLoc, int[] endLoc)
    {
        LinkedList<List<int[]>> q = new LinkedList<List<int[]>>();
        HashSet<string> v = new HashSet<string>();

        List<int[]> firstElement = new List<int[]>();
        firstElement.Add(startLoc);
        q.AddFirst(firstElement);

        int[,] searchDirections = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        while (q.Count > 0)
        {
            List<int[]> cur = q.First.Value;

            int x = cur[cur.Count - 1][0];
            int y = cur[cur.Count - 1][1];
            if (v.Contains(x + "," + y))
            {
                q.RemoveFirst();
                continue;
            }

            if (x == endLoc[0] && y == endLoc[1])
            {
                return ConvertIntArrList(cur);
            }

            for (int i = 0; i < searchDirections.GetLength(0); i++)
            {
                int newX = x + searchDirections[i, 0];
                int newY = y + searchDirections[i, 1];

                if (newY >= 0 && newY < grid.GetLength(0) &&
                    newX >= 0 && newX < grid.GetLength(1) &&
                    (grid[newY, newX] == 0 || (newX == endLoc[0] && newY == endLoc[1])) &&
                    !v.Contains(newX + "," + newY))
                {
                    List<int[]> newList = new List<int[]>(cur);
                    newList.Add(new int[] { newX, newY });
                    q.AddLast(newList);
                }
            }

            v.Add(x + "," + y);
            if (q.Count == 1)
            {
                this.target = null;
                return ConvertIntArrList(q.First.Value);
            }
            q.RemoveFirst();
        }


        Debug.Log("No Path Found " + q.Count);

        return new List<Vector2>();
    }

    protected void UnsubscribeToPath()
    {
        if (path == null)
        {
            return;
        }
        foreach (Vector2 point in path)
        {
            int[] gridLoc = Map.WorldPointToGridPoint(point);
            string key = gridLoc[0] + "," + gridLoc[1];
            Map.PathTakers[key].Remove(this);

        }
    }

    protected void SubscribeToPath()
    {
        foreach (Vector2 point in path)
        {
            int[] gridLoc = Map.WorldPointToGridPoint(point);
            string key = gridLoc[0] + "," + gridLoc[1];
            if (!Map.PathTakers.ContainsKey(key))
            {
                Map.PathTakers.Add(key, new HashSet<Zombie>());
            }
            Map.PathTakers[key].Add(this);
        }
    }


    private bool hasAlreadyDied = false;
    public void TakeDamage(int damage)
    {
        this.health -= damage;
        if (health <= 0 && !hasAlreadyDied)
        {
            hasAlreadyDied = true;
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Purchaser.Give(this.KillReward);
        Destroy(this.gameObject);
    }

    private List<Vector2> ConvertIntArrList(List<int[]> input)
    {
        List<Vector2> output = new List<Vector2>();
        for (int i = 0; i < input.Count; i++)
        {
            output.Add(Map.GridPointToWorldPoint(input[i]));
        }
        return output;
    }
}



