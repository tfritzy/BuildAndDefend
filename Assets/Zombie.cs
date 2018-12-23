using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    protected List<Vector2> path;
    protected Builder builder;
    protected int[] locationInGrid;
    protected int[] targetLoc;
    protected bool atFinalLoc = false;
    protected int pathProgress = 0;
    protected int damage;
    protected float lastAttackTime;
    protected float attackSpeed;

    public float zombieSpeed = .3f;
    public int health = 5;
    public GameObject target;


    // Use this for initialization
    void Start () {
        this.path = new List<Vector2>();
        this.builder = GameObject.Find("BuildModeButton").GetComponent<Builder>();
        lastAttackTime = Time.time;
        RestartPath();
        ChildrenSetup();
    }
	
	// Update is called once per frame
	void Update () {
        FollowPath();
        Attack();
	}

    protected void FollowPath()
    {
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
        if ((path[pathProgress] - (Vector2)transform.position).magnitude < .6f){
            pathProgress += 1;
            if (pathProgress >= path.Count)
            {
                Debug.Log("Reached End");
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
            Debug.Log("Path: " + path.Count);
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
        SubscribeToPath();
    }

    public virtual List<Vector2> RestartPath()
    {
        pathProgress = 0;
        UnsubscribeToPath();
        this.target = GameObject.Find("Turret");
        this.locationInGrid = builder.WorldPointToGridPoint(this.transform.position);
        this.targetLoc = new int[] { 16, 8 };
        path = FindPath(builder.grid, locationInGrid, this.targetLoc);
        SubscribeToPath();
        return path;
    }

    // Perform BFS to find shortest path to the desired location
    public List<Vector2> FindPath(byte[,] grid, int[] startLoc, int[] endLoc)
    {
        Debug.Log("X: " + endLoc[0] + " y: " + endLoc[1]);
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
        foreach (Vector2 point in path)
        {
            int[] gridLoc = builder.WorldPointToGridPoint(point);
            string key = gridLoc[0] + "," + gridLoc[1];
            builder.pathTakers[key].Remove(this);
        }
    }

    protected void SubscribeToPath()
    {
        foreach(Vector2 point in path)
        {
            int[] gridLoc = builder.WorldPointToGridPoint(point);
            string key = gridLoc[0] + "," + gridLoc[1];
            if (!builder.pathTakers.ContainsKey(key)){
                builder.pathTakers.Add(key, new HashSet<Zombie>());
            }
            builder.pathTakers[key].Add(this);
        }
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
        //Debug.Log(this.health);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private List<Vector2> ConvertIntArrList(List<int[]> input)
    {
        List<Vector2> output = new List<Vector2>();
        for (int i = 0; i < input.Count; i++)
        {
            output.Add(builder.GridPointToWorldPoint(input[i]));
        }
        return output;
    }
}



