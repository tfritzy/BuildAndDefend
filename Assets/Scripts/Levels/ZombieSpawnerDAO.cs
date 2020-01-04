public class ZombieSpawnerDAO {
    public int XPos;
    public int YPos;
    public float SpawnRate;
    public UnityEngine.Vector2 Pos { get { return new UnityEngine.Vector2(XPos, YPos); } }
}