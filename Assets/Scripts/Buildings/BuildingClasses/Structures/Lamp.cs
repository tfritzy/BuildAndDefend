using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Building {

    private Darkness darkness;
    public float strength = .8f;

	public Lamp()
    {
        this.WoodCost = 150;
        this.StructPath = "Gameobjects/Buildings/Lamp";
    }

    protected override void OnDeath()
    {
        Vector2 curPos = this.transform.position;
        Vector2 gridPos = darkness.WorldPointToGridPoint(curPos.x, curPos.y);
        darkness.RemoveLight((int)gridPos.x, (int)gridPos.y);
    }

    protected override void Setup()
    {
        this.darkness = GameObject.Find("Night").GetComponent<Darkness>();
        Vector2 curPos = this.transform.position;
        Vector2 gridPos = darkness.WorldPointToGridPoint(curPos.x, curPos.y);
        darkness.AddLight((int)gridPos.x, (int)gridPos.y, 10, strength);
    }




}
