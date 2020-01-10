using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimer : MonoBehaviour {

    public int numWaves;
    public GameObject waveTick;

    private Vector2 waveBarLoc;
    private RectTransform waveBar;
    private float xVelocity;
    private float barWidth;
    private float waveLength = 30f;

	// Use this for initialization
	void Start () {
        waveBarLoc = GameObject.Find("WaveTimeline").transform.position;
        waveBar = GameObject.Find("WaveTimeline").GetComponent<RectTransform>();
        barWidth = waveBar.rect.width;
        SetNumWaves(3);
	}
	
	// Update is called once per frame
	void Update () {
        MoveIndexLocation();
	}

    void MoveIndexLocation()
    {
        Vector2 curLoc = (Vector2)this.transform.position;
        curLoc.x += Time.deltaTime * xVelocity;
        this.transform.position = curLoc;
    }

    void SetNumWaves(int count)
    {
        numWaves = count;
        xVelocity = barWidth / (numWaves * waveLength);

        float pixelWidth = GameObjectCache.Camera.pixelWidth;
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = GameObjectCache.Camera.ViewportToWorldPoint(topRightCorner);
        float widthWorldPoints = edgeVector.x * 2;

        float distBetweenTicks = ((barWidth / numWaves) / pixelWidth) * widthWorldPoints;
        Debug.Log(distBetweenTicks);
        Vector2 tickLoc = waveBar.transform.position;
        tickLoc.x += distBetweenTicks;
        for (int i = 0; i < numWaves; i++)
        {
            GameObject inst = Instantiate(waveTick, tickLoc, new Quaternion(), this.transform.parent);
            RectTransform instRect = inst.GetComponent<RectTransform>();
            instRect.SetPositionAndRotation(tickLoc, new Quaternion());
            tickLoc.x += distBetweenTicks;
        }
        
    }
}
