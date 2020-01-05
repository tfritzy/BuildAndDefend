public class WireTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Water;
    public override PathableType PathableType => PathableType.Pathable;
    public override bool CanBeBuiltUpon => false;
}