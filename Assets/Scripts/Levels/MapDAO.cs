using System.Collections.Generic;

public class MapDAO
{
    public string name;
    public MapRewardsDAO rewards;
    public EnvironmentTileType[] environment;
    public List<ZombieSpawnerDAO> zombieSpawners;
}