using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkness : MonoBehaviour
{

    private Transform night;
    private GridLight[,] darknessGrid;
    private float xStart;
    private float yStart;
    private float gridPlotWidth;
    private float gridPlotHeight;
    private Dictionary<string, LightSource> sources = new Dictionary<string, LightSource>();
    private LevelManager levelManager;
    private float lastNightSegmentTime;
    private float nightSegmentLength;
    private bool isDuringNight = false;
    private float amountOfLightChange;

    public GameObject darknessImage;

    private void Awake()
    {
        this.night = GameObject.Find("Night").GetComponent<Transform>();
    }

    void Start()
    {
        this.darknessGrid = new GridLight[Map.Environment.GetLength(0), Map.Environment.GetLength(1)];
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        this.xStart = canvas.pixelRect.width / 2 * -1;
        this.yStart = canvas.pixelRect.height / 2 * -1;
        this.gridPlotWidth = canvas.pixelRect.width / darknessGrid.GetLength(1);
        this.gridPlotHeight = canvas.pixelRect.height / darknessGrid.GetLength(0);
        darknessImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25);
        darknessImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25);

        Debug.Log(darknessGrid.GetLength(0) + " by " + darknessGrid.GetLength(1));
        Debug.Log("XstartandYStart: " + "(" + xStart + "," + yStart + ")" + " " + "(" + gridPlotWidth + "," + gridPlotHeight + ")");
        this.levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        nightSegmentLength = levelManager.levelDuration / 120f;
        amountOfLightChange = .5f / 60f;
        InitiateNight();
        EndNight();
    }

    private void Update()
    {
        ProgressThroughNight();
    }

    public void StartNight()
    {
        this.isDuringNight = true;
        SetSunlight(.5f);
    }

    public void EndNight()
    {
        this.isDuringNight = false;
        SetSunlight(1f);
    }


    private void ProgressThroughNight()
    {
        if (!isDuringNight)
        {
            return;
        }
        if (Time.time < levelManager.levelStartTime + levelManager.levelDuration / 2f)
        {
            if (Time.time > lastNightSegmentTime + nightSegmentLength)
            {
                DecreaseSunlight(amountOfLightChange);
                lastNightSegmentTime = Time.time;
            }
        }
        else
        {
            if (Time.time > lastNightSegmentTime + nightSegmentLength)
            {
                IncreaseSunlight(amountOfLightChange * 2);
                lastNightSegmentTime = Time.time;
            }
        }
    }

    public void SetSunlight(float strength)
    {
        if (sources.ContainsKey("sun"))
        {
            LightSource sun = sources["sun"];
            DecreaseSunlight(sun.strength);
            sources.Remove("sun");
        }
        sources.Add("sun", new LightSource(-1, -1, 1000, 0));
        IncreaseSunlight(strength);
    }

    public void IncreaseSunlight(float amount)
    {
        LightSource sun = sources["sun"];
        sun.strength += amount;
        foreach (GridLight light in darknessGrid)
        {
            light.colorOfSpot -= amount;
            light.SetColor();
        }
    }

    public void DecreaseSunlight(float amount)
    {
        LightSource sunlight = sources["sun"];
        sunlight.strength -= amount;
        foreach (GridLight light in darknessGrid)
        {
            light.colorOfSpot += amount;
            light.SetColor();
        }

    }

    public void RemoveLight(int x, int y)
    {

        if (!sources.ContainsKey(x + "," + y))
        {
            Debug.Log("Light not in lights :(");
            return;
        }
        LightSource currentLight = sources[x + "," + y];
        ModifyCircle(x, y, currentLight.radius, currentLight.strength, true);
        sources.Remove(x + "," + y);
        Debug.Log("Removed Light at: (" + x + "," + y + ")");
    }

    public void AddLight(int x, int y, int r, float strength)
    {
        Debug.Log("Light added at: (" + x + "," + y + ")");
        LightSource newLight = new LightSource(x, y, r, strength);
        sources.Add(x + "," + y, newLight);
        ModifyCircle(x, y, r, strength, false);
    }

    private void ModifyCircle(int x, int y, int r, float strength, bool removing)
    {
        for (int i = x - r; i <= x + r; i++)
        {
            for (int j = y - r; j <= y + r; j++)
            {
                float dist = (new Vector2(i, j) - new Vector2(x, y)).magnitude;

                if ((int)dist <= r)
                {
                    if (!CheckBounds(i, j))
                        continue;
                    dist = dist / 30f;
                    Color currentColor = darknessGrid[j, i].imageOnSpot.GetComponent<Image>().color;
                    if (removing)
                        darknessGrid[j, i].colorOfSpot += (strength - (float)Mathf.Min(strength, dist));
                    else
                        darknessGrid[j, i].colorOfSpot -= (strength - (float)Mathf.Min(strength, dist));
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
                           ShadowGridLocToWorldSpace(i, j),
                           new Quaternion(),
                           night);
                darknessGrid[j, i] = new GridLight(darkInst);
            }
        }
    }

    public Vector2 ShadowGridLocToWorldSpace(int x, int y)
    {
        Vector2 pixelCoords = new Vector2(((float)x) * this.gridPlotWidth + xStart,
                                          ((float)y) * this.gridPlotHeight + yStart);
        return Camera.main.ScreenToWorldPoint(pixelCoords);
    }

    public Vector2 WorldPointToGridPoint(float x, float y)
    {
        return new Vector2((int)(((float)x + 10 - .25f) * 2),
                           (int)(((float)y + 6 - .25f) * 2));
    }

}
