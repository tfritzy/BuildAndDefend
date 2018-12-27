﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Building {

    private Darkness darkness;

    public float strength = .4f;

	public Lamp()
    {
        this.woodCost = 150;
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
        darkness.AddLight((int)gridPos.x, (int)gridPos.y, 5, strength);
    }
}
