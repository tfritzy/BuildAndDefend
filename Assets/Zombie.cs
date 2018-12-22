using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    private List<Vector2> path;
    private Builder builder;
    private int[] locationInGrid;
    private int[] targetLoc;
    private int pathProgress = 0;
    private bool atFinalLoc = false;

    public float zombieSpeed = .3f;
    public GameObject testRedDot;
    public int health = 5;
    

	// Use this for initialization
	void Start () {
        this.path = new List<Vector2>();
        this.builder = GameObject.Find("BuildModeButton").GetComponent<Builder>();
        RestartPath();
        //Debug.Log("Path Length: " + path.Count);
        foreach (Vector2 coord in path)
        {
            //Instantiate(testRedDot, coord, new Quaternion());
        }
    }
	
	// Update is called once per frame
	void Update () {
        FollowPath();
	}

    void FollowPath()
    {
        if (atFinalLoc)
        {
            return;
        }
        if (path == null || path.Count <= 0)
        {
            return;
        }
        //Debug.Log("Target: " + path[pathProgress]);
        
        this.transform.position = Vector2.MoveTowards(this.transform.position, path[pathProgress], zombieSpeed * Time.deltaTime);
        if ((path[pathProgress] - (Vector2)this.transform.position).magnitude < .2f){
            pathProgress += 1;
            if (pathProgress >= path.Count)
            {
                atFinalLoc = true;
            }
        }
    }

    public void SetPath(List<Vector2> newPath)
    {
        pathProgress = 0;
        UnsubscribeToPath();
        this.path = newPath;
        SubscribeToPath();
    }

    public List<Vector2> RestartPath()
    {
        pathProgress = 0;
        UnsubscribeToPath();
        this.locationInGrid = builder.WorldPointToGridPoint(this.transform.position);
        this.targetLoc = new int[] { 16, 8 };
        path = FindPath(builder.grid, locationInGrid, this.targetLoc);
        SubscribeToPath();
        return path;
    }

    // Perform BFS to find shortest path to the desired location
    public List<Vector2> FindPath(bool[,] grid, int[] startLoc, int[] endLoc)
    {
        LinkedList<List<int[]>> q = new LinkedList<List<int[]>>();
        HashSet<string> v = new HashSet<string>();

        List<int[]> firstElement = new List<int[]>();
        firstElement.Add(startLoc);
        q.AddFirst(firstElement);

        int[,] searchDirections = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        while (q.Count > 0)
        {
            //Debug.Log("Queue Length: " + q.Count);
            List<int[]> cur = q.First.Value;
            
            int x = cur[cur.Count - 1][0];
            int y = cur[cur.Count - 1][1];
            //Debug.Log("Current: " + x + "," + y);
            if (v.Contains(x + "," + y))
            {
                //Debug.Log("Removed: " + x + " and " + y);
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
                //Debug.Log("TestPos: " + newX + " " + newY);

                if (newY >= 0 && newY < grid.GetLength(0) &&
                    newX >= 0 && newX < grid.GetLength(1) &&
                    !grid[newY, newX] &&
                    !v.Contains(newX + "," + newY))
                {
                    //Debug.Log("Added: " + newX + " and " + newY);
                    List<int[]> newList = new List<int[]>(cur);
                    newList.Add(new int[] { newX, newY });
                    q.AddLast(newList);
                } 
            }
            //Debug.Log("Removed: " + x + " and " + y);
            v.Add(x + "," + y);
            if (q.Count == 1)
            {
                return ConvertIntArrList(q.First.Value);
            }
            q.RemoveFirst();
        }

        // No Path Found :(
        Debug.Log("No Path Found " + q.Count);
        
        return new List<Vector2>();
    }

    private void UnsubscribeToPath()
    {
        foreach (Vector2 point in path)
        {
            int[] gridLoc = builder.WorldPointToGridPoint(point);
            string key = gridLoc[0] + "," + gridLoc[1];
            builder.pathTakers[key].Remove(this);
        }
    }

    private void SubscribeToPath()
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



