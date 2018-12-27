using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource {
    public int radius;
    public float strength;
    public int x;
    public int y;
    public LightSource(int x, int y, int radius, float strength)
    {
        this.radius = radius;
        this.strength = strength;
        this.x = x;
        this.y = y;
    }
}
