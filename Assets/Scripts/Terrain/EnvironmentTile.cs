using UnityEngine;

public abstract class EnvironmentTile : MonoBehaviour
{
    public abstract EnvironmentTileType Type { get; }
    public abstract PathableType PathableType { get; }
    public abstract bool CanBeBuiltUpon { get; }
}