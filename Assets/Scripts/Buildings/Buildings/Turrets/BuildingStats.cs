
using System;

[Serializable]
public class BuildingStats
{
    public int Health;
    public BuildingStats(int health = 0)
    {
        this.Health = health;
    }
}