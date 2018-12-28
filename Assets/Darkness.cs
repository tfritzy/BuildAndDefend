using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkness : MonoBehaviour {

    private Builder builder;
    private Transform night;
    private GridLight[,] darknessGrid;
    private float xStart = -20f;
    private float yStart = -12f;
    private Dictionary<string, LightSource> sources = new Dictionary<string, LightSource>();

    public GameObject darknessImage;

    private void Awake()
    {
        this.builder = GameObject.Find("BuildModeButton").GetComponent<Builder>();
        this.night = GameObject.Find("Night").GetComponent<Transform>();

    }

    void Start() {
        this.darknessGrid = new GridLight[builder.grid.GetLength(0) + 10, builder.grid.GetLength(1) + 10];
        InitiateNight();
        //AddLight(46, 16, 8, .8f);
        //AddLight(15, 15, 8, .8f);
        
    }

    public void RemoveLight(int x, int y)
    {
        Debug.Log("Removed Light at: (" + x + "," + y + ")");
        if (!sources.ContainsKey(x + "," + y))
        {
            Debug.Log("Light not in lights :(");
            return;
        }
        LightSource currentLight = sources[x + "," + y];
        ModifyCircle(x, y, currentLight.radius, -1 * currentLight.strength);
        sources.Remove(x + "," + y);
    }

    public void AddLight(int x, int y, int r, float strength)
    {
        Debug.Log("Light added at: (" + x + "," + y + ")");
        LightSource newLight = new LightSource(x, y, r, strength);
        sources.Add(x + "," + y, newLight);
        ModifyCircle(x, y, r, strength);
    }

    private void ModifyCircle(int x, int y, int r, float strength)
    {

        for (int i = x-r; i <= x+r; i++)
        {
            for (int j = y-r; j <= y+r; j++)
            {
                float dist = (new Vector2(i, j) - new Vector2(x, y)).magnitude;

                if ((int)dist <= r)
                {
                    if (!CheckBounds(i, j))
                        continue;
                    dist = dist / 15f;
                    Color currentColor = darknessGrid[j, i].imageOnSpot.GetComponent<Image>().color;
                    darknessGrid[j, i].colorOfSpot -= Mathf.Max(0, (strength - dist));
                    darknessGrid[j, i].SetColor();
                }
            }
        }

    }

    private bool CheckBounds(int x, int y)
    {
        
        if (x < 0 || x >= darknessGrid.GetLength(1))
        {
            return false;
        }
        else if (y < 0 || y >= darknessGrid.GetLength(0))
        {
            return false;
        }
        return true;
    }

    void InitiateNight()
    {
        for (int i = 0; i < darknessGrid.GetLength(1); i++)
        {
            for (int j = 0; j < darknessGrid.GetLength(0); j++)
            {
                GameObject darkInst = Instantiate(darknessImage,
                           ShadowGridLocToWorldSpace(i,j),
                           new Quaternion(), 
                           night);
                darknessGrid[j, i] = new GridLight(darkInst);
            }
        }
    }

    public Vector2 ShadowGridLocToWorldSpace(int x, int y)
    {
        return new Vector2(((float)x + xStart) / 2f + .5f,
                           ((float)y + yStart) / 2f + .5f);
    }

    public Vector2 WorldPointToGridPoint(float x, float y)
    {
        return new Vector2((int)(((float)x + 10 - .25f) * 2),
                           (int)(((float)y + 6 - .25f) * 2));
    }

}
