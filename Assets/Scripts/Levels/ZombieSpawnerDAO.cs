public class ZombieSpawnerDAO
{
    public int XPos;
    public int YPos;
    public float SpawnRate;
    public UnityEngine.Vector2Int Pos { get { return new UnityEngine.Vector2Int(XPos, YPos); } }
}