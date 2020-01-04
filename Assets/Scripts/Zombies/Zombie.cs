﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    protected List<Vector2> path;
    protected Vector2Int locationInGrid;
    public Vector2Int targetLoc;
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
        (this.targetLoc, this.target) = findClosestTower();
        this.locationInGrid = Map.WorldPointToGridPoint(this.transform.position);
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

    private (Vector2Int, GameObject) findClosestTower()
    {
        int closestDist = int.MaxValue;
        Vector2Int closestPos = Vector2Int.zero;
        GameObject closestTarget = null;
        foreach (string towerPos in Map.Towers.Keys)
        {
            int distance = Mathf.Abs((towerPos.ToVector2Int().x - this.locationInGrid.x)) + Mathf.Abs((towerPos.ToVector2Int().y - this.locationInGrid.y));
            if (distance < closestDist)
            {
                closestDist = distance;
                closestTarget = Map.Towers[towerPos];
                closestPos = towerPos.ToVector2Int();
            }
        }
        return (closestPos, closestTarget);
    }

    // Perform BFS to find shortest path to the desired location
    public List<Vector2> FindPath(TileType[,] grid, Vector2Int startLoc, Vector2Int endLoc)
    {
        LinkedList<List<Vector2Int>> q = new LinkedList<List<Vector2Int>>();
        HashSet<string> v = new HashSet<string>();

        List<Vector2Int> firstElement = new List<Vector2Int>();
        firstElement.Add(startLoc);
        q.AddFirst(firstElement);

        int[,] searchDirections = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        while (q.Count > 0)
        {
            List<Vector2Int> cur = q.First.Value;

            int x = cur[cur.Count - 1][0];
            int y = cur[cur.Count - 1][1];
            if (v.Contains(x + "," + y))
            {
                q.RemoveFirst();
                continue;
            }

            if (x == endLoc[0] && y == endLoc[1])
            {
                return ConvertGridPointListToWorldPoint(cur);
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
                    List<Vector2Int> newList = new List<Vector2Int>(cur);
                    newList.Add(new Vector2Int(newX, newY));
                    q.AddLast(newList);
                }
            }

            v.Add(x + "," + y);
            if (q.Count == 1)
            {
                this.target = null;
                return ConvertGridPointListToWorldPoint(q.First.Value);
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
            Vector2Int gridLoc = Map.WorldPointToGridPoint(point);
            string key = gridLoc[0] + "," + gridLoc[1];
            Map.PathTakers[key].Remove(this);

        }
    }

    protected void SubscribeToPath()
    {
        foreach (Vector2 point in path)
        {
            Vector2Int gridLoc = Map.WorldPointToGridPoint(point);
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

    /// <summary>
    /// The logic to perform when this zombie dies.
    /// </summary>
    protected virtual void OnDeath()
    {
        Purchaser.Give(this.KillReward);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Takes a list of grid points, and turns it into a list of world points.static
    /// Is used to turn a zombie's grid path into a world point path.
    /// </summary>
    /// <param name="gridPointList"></param>
    /// <returns></returns>
    private List<Vector2> ConvertGridPointListToWorldPoint(List<Vector2Int> gridPointList)
    {
        List<Vector2> worldPointList = new List<Vector2>();
        foreach (Vector2Int gridPoint in gridPointList)
        {
            worldPointList.Add(Map.GridPointToWorldPoint(gridPoint));
        }
        return worldPointList;
    }
}



