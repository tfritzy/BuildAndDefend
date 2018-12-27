using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkness : MonoBehaviour {

    private Builder builder;
    private Transform night;
    private GameObject[,] darknessGrid;
    private float xStart = -20f;
    private float yStart = -12f;
    private Dictionary<string, LightSource> sources = new Dictionary<string, LightSource>();

    public GameObject darknessImage;


	void Start () {
        this.builder = GameObject.Find("BuildModeButton").GetComponent<Builder>();
        this.night = GameObject.Find("Night").GetComponent<Transform>();
        this.darknessGrid = new GameObject[builder.grid.GetLength(0) + 10, builder.grid.GetLength(1) + 10];
        InitiateNight();
        AddLight(15, 15, 8, .8f);
    }

    public void RemoveLight(int x, int y)
    {
        if (sources.ContainsKey(x + "," + y))
        {
            return;
        }
        LightSource currentLight = sources[x + "," + y];
        ModifyCircle(x, y, currentLight.radius, -1 * currentLight.strength);
        sources.Remove(x + "," + y);
    }

    public void AddLight(int x, int y, int r, float strength)
    {
        LightSource newLight = new LightSource(x, y, r, strength);
        sources.Add(x + "," + y, newLight);
        ModifyCircle(x, y, r, strength);
    }

    private void ModifyCircle(int x, int y, int r, float strength)
    {
        for (int i = y - r; i <= y + r; i++)
        {
            for (int j = x; (j - x) * (j - x) + (i - y) * (i - y) < r * r; j--)
            {
                float colorModifier = (new Vector2(i, j) - new Vector2(x, y)).magnitude;
                colorModifier = colorModifier / 15f;
                Color currentColor = darknessGrid[i, j].GetComponent<Image>().color;
                currentColor.a -= Mathf.Max(0, (strength - colorModifier));
                darknessGrid[i, j].GetComponent<Image>().color = currentColor;
            }
            for (int j = x + 1; (j - x) * (j - x) + (i - y) * (i - y) < r * r; j++)
            {
                float colorModifier = (new Vector2(i, j) - new Vector2(x, y)).magnitude;
                colorModifier = colorModifier / 15f;
                Color currentColor = darknessGrid[i, j].GetComponent<Image>().color;
                currentColor.a -= Mathf.Max(0, (strength - colorModifier));
                darknessGrid[i, j].GetComponent<Image>().color = currentColor;
            }
        }
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
                darknessGrid[j, i] = darkInst;
            }
        }
    }

    public Vector2 ShadowGridLocToWorldSpace(int x, int y)
    {
        return new Vector2((x + xStart) / 2f + .5f,
                           (y + yStart) / 2f + .5f);
    }

    public Vector2 WorldPointToGridPoint(float x, float y)
    {
        return new Vector2((int)((x - xStart - .25f) * 2),
                           (int)((y - yStart - .25f) * 2));
    }

}
