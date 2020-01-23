using System.Collections.Generic;

public class MapDAO
{
    public string name;
    public MapRewardsDAO rewards;
    public int width;
    public int height;
    public EnvironmentTileType[] environment;
    public List<ZombieSpawnerDAO> zombieSpawners;
    public List<BuildingOnMapDAO> buildings;
}