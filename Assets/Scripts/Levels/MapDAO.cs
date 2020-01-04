using System.Collections.Generic;

public class MapDAO
{
    public string name;
    public MapRewardsDAO rewards;
    public TileType[] grid;
    public List<ZombieSpawnerDAO> zombieSpawners;
}