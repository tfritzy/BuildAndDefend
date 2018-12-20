using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour {

    // Private fields
    float blockSize;
    float wallZAxis = 0f;
    private GameObject selectedBlock;

    // Pulic Fields
    public GameObject wallSegment;
    public bool[,] grid;
    public GameObject water;
    public GameObject brush;
    public GameObject chasm;
    public GameObject wire;
    


    private void Awake()
    {
        this.grid = new bool[32, 64];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SelectWater()
    {
        this.selectedBlock = water;
    }

    public void SelectBruch()
    {
        this.selectedBlock = brush;
    }
    
    public void SelectWire()
    {
        this.selectedBlock = wire;
    }


    
}
