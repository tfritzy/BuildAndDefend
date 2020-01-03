using System.Collections.Generic;

public class MapDAO
{
    public string name;
    public MapRewardsDAO rewards;
    public byte[] grid;
    public List<ZombieSpawnerDAO> zombieSpawners;
}