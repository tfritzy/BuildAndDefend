
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    protected List<Vector2> path;
    protected Vector2Int locationInGrid;
    public Vector2Int targetLoc;
    protected int pathProgress = 0;
    protected int damage = 5;
    protected float lastAttackTime;
    protected float attackSpeed = 1;
    private GameObject healthbar;
    private float originalHealthbarScale;

    public float zombieSpeed = .3f;
    public int health = 50;
    protected int startingHealth;
    public GameObject target;
    public virtual ResourceDAO KillReward { get => new ResourceDAO(gold: 10); }
    public virtual int XP => 1;
    public static float lastZombieFindPathTime;
    public ZombieState CurrentState;

    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        AILoop();
    }

    /// <summary>
    /// Let this zombie know that its path was broken and it needs to recalculate.
    /// </summary>
    public void NotifyOfPathBreak()
    {
        this.path = null;
        this.CurrentState = ZombieState.LookingForPathToTower;
    }

    protected async void AILoop()
    {
        switch (this.CurrentState)
        {
            case (ZombieState.LookingForPathToNearestBuilding):
            case (ZombieState.LookingForPathToTower):
                await RestartPath();
                break;
            case (ZombieState.AttackingTarget):
                Attack();
                break;
            case (ZombieState.FollowingPathToNearestBuilding):
            case (ZombieState.FollowingPathToTower):
                moveTowardsNextPathPoint();
                break;
        }
    }

    protected virtual void Setup()
    {
        this.CurrentState = ZombieState.LookingForPathToTower;
        this.path = null;
        lastAttackTime = Time.time;
        this.healthbar = this.transform.Find("Healthbar").gameObject;
        originalHealthbarScale = this.healthbar.transform.localScale.y;
        this.startingHealth = health;
    }

    private void moveTowardsNextPathPoint()
    {
        this.transform.position = Vector2.MoveTowards(transform.position, path[pathProgress], zombieSpeed * Time.deltaTime);
        if (Vector3.Distance(path[pathProgress], transform.position) < .51f)
        {
            pathProgress += 1;
            if (pathProgress >= path.Count)
            {
                this.CurrentState = ZombieState.AttackingTarget;
            }
        }
    }

    async void Attack()
    {
        if (this.target == null)
        {
            this.path = null;
            this.CurrentState = ZombieState.LookingForPathToTower;
            UnsubscribeToPath();
            return;
        }
        this.transform.position = Vector2.MoveTowards(transform.position, target.transform.position, zombieSpeed * Time.deltaTime);

        if (Time.time < lastAttackTime + attackSpeed)
        {
            return;
        }

        if (target != null)
        {
            target.GetComponent<Building>().TakeDamage(this.damage);
        }
        else
        {
            await RestartPath();
        }
        lastAttackTime = Time.time;
    }

    protected bool IsCurrentPathIsValid()
    {
        if (this.path == null)
        {
            return false;
        }
        for (int i = 0; i < this.path.Count - 1; i++)
        {
            Vector2Int gridPos = Map.WorldPointToGridPoint(this.path[i]);
            if (Map.PathingGrid[gridPos.x, gridPos.y] == PathableType.UnPathable)
            {
                return false;
            }
        }
        return true;
    }

    public void NotifyOfPotentialPath(List<Vector2> newPath)
    {
        if (this.IsCurrentPathIsValid() || !needsNewPath())
        {
            return;
        }
        SetPath(newPath);
    }

    public void SetPath(List<Vector2> newPath)
    {
        if (!needsNewPath())
        {
            return;
        }
        pathProgress = 0;
        UnsubscribeToPath();
        this.path = newPath;
        SubscribeToPath();
    }

    protected bool needsNewPath()
    {
        return (this.CurrentState == ZombieState.LookingForPathToNearestBuilding ||
                this.CurrentState == ZombieState.LookingForPathToTower);
    }

    public async virtual Task<List<Vector2>> RestartPath()
    {
        if (Time.time < Zombie.lastZombieFindPathTime + .05f)
        {
            return await Task.FromResult<List<Vector2>>(null);
        }

        UnsubscribeToPath();

        if (this.CurrentState == ZombieState.LookingForPathToTower)
        {
            (this.targetLoc, this.target) = findClosestBuilding(findOnlyTowers: true);
        }
        else if (this.CurrentState == ZombieState.LookingForPathToNearestBuilding)
        {
            (this.targetLoc, this.target) = findClosestBuilding(findOnlyTowers: false);
        }

        // Couldn't find any buildings on the map.
        if (this.target == null)
        {
            this.CurrentState = ZombieState.LookingForPathToTower;
            return null;
        }

        this.locationInGrid = Map.WorldPointToGridPoint(this.transform.position);
        this.path = await FindPath(locationInGrid, this.targetLoc);

        // If FindPath 
        if (this.path == null)
        {
            if (this.CurrentState == ZombieState.LookingForPathToTower)
            {
                this.CurrentState = ZombieState.LookingForPathToNearestBuilding;
            }
            else if (this.CurrentState == ZombieState.LookingForPathToNearestBuilding)
            {
                // If the zombie is trapped, There isn't much it can do.
                // TODO Make the zombie perform a bfs to find the closest building.
                // This would be far more likely to produce the correct result.
                Destroy(this.gameObject);
            }
            return null;
        }
        else
        {
            if (this.CurrentState == ZombieState.LookingForPathToTower)
            {
                this.CurrentState = ZombieState.FollowingPathToTower;
            }
            else if (this.CurrentState == ZombieState.LookingForPathToNearestBuilding)
            {
                this.CurrentState = ZombieState.FollowingPathToNearestBuilding;
            }
        }

        Collider2D[] nearbyZombs = Physics2D.OverlapCircleAll(this.transform.position, 1f);
        foreach (Collider2D col in nearbyZombs)
        {
            if (col.tag != "Zombie" || col.gameObject == this.gameObject)
                continue;

            Zombie colZomb = col.GetComponent<Zombie>();
            colZomb.NotifyOfPotentialPath(this.path);
        }

        SubscribeToPath();
        return path;
    }

    private (Vector2Int, GameObject) findClosestBuilding(bool findOnlyTowers = false)
    {
        float closestDist = float.MaxValue;
        Vector2Int closestPos = Vector2Int.zero;
        GameObject closestTarget = null;
        Vector2Int zombieGridPos = Map.WorldPointToGridPoint(this.transform.position);
        foreach (string towerId in Map.BuildingDict.Keys)
        {
            if (findOnlyTowers && Map.BuildingDict[towerId].GetComponent<Tower>() == null)
            {
                continue;
            }

            Vector2Int size = Map.BuildingDict[towerId].GetComponent<Building>().Size;
            Vector2Int closestPosToZombie = Map.BuildingDict[towerId].GetClosestPointOnBuildingToGridPosition(zombieGridPos);
            float lineOfSightDistance = Vector2Int.Distance(closestPos, zombieGridPos);
            if (lineOfSightDistance < closestDist)
            {
                closestDist = lineOfSightDistance;
                closestPos = closestPosToZombie;
                closestTarget = Map.BuildingDict[towerId].gameObject;
            }
        }
        return (closestPos, closestTarget);
    }

    protected static int[,] searchDirections = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
    // Perform BFS to find shortest path to the desired location
    public Task<List<Vector2>> FindPath(Vector2Int startLoc, Vector2Int endLoc, bool ignoreBuildings = false)
    {
        Zombie.lastZombieFindPathTime = Time.time;
        return Task.Run(() =>
        {
            LinkedList<List<Vector2Int>> q = new LinkedList<List<Vector2Int>>();
            HashSet<string> v = new HashSet<string>();

            List<Vector2Int> firstElement = new List<Vector2Int>();
            firstElement.Add(startLoc);
            q.AddFirst(firstElement);

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
                    if (!IsWithinBounds(newX, newY))
                    {
                        continue;
                    }

                    if ((Map.PathingGrid[newX, newY] == PathableType.Pathable ||
                        (newX == endLoc[0] && newY == endLoc[1])) &&
                        !v.Contains(newX + "," + newY))
                    {
                        List<Vector2Int> newList = new List<Vector2Int>(cur);
                        newList.Add(new Vector2Int(newX, newY));
                        q.AddLast(newList);
                    }
                }

                v.Add(x + "," + y);
                q.RemoveFirst();
            }

            return null;
        });
    }

    private bool IsPathable(int x, int y)
    {
        return Map.PathingGrid[x, y] == PathableType.Pathable;
    }

    private bool IsWithinBounds(int x, int y)
    {
        if (y >= 0 &&
            y < Map.PathingGrid.GetLength(1) &&
            x >= 0 &&
            x < Map.PathingGrid.GetLength(0))
        {
            return true;
        }
        return false;
    }

    protected void UnsubscribeToPath()
    {
        pathProgress = 0;
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
        this.path = null;
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

    protected void updateHealthbar()
    {
        Vector3 scale = this.healthbar.transform.localScale;
        scale.y = ((float)this.health / (float)startingHealth) * this.originalHealthbarScale;
        this.healthbar.transform.localScale = scale;
    }


    private bool hasAlreadyDied = false;
    public void TakeDamage(int damage, Tower attacker)
    {
        this.health -= damage;
        Player.PlayerData.Values.BuildingUpgrades[attacker.Type].DamageDealt += damage;
        Player.PlayerData.Values.BuildingUpgrades[attacker.Type].XP += this.XP;
        updateHealthbar();

        if (health <= 0 && !hasAlreadyDied)
        {
            hasAlreadyDied = true;
            OnDeath(attacker);
        }
    }

    /// <summary>
    /// The logic to perform when this zombie dies.
    /// </summary>
    protected virtual void OnDeath(Tower attacker)
    {
        Purchaser.Give(this.KillReward);
        Player.PlayerData.Values.BuildingUpgrades[attacker.Type].Kills += 1;
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



