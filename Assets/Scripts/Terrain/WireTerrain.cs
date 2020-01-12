public class WireTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Water;
    public override PathableType PathableType => PathableType.UnPathable;
    public override bool CanBeBuiltUpon => false;
    public override bool StopsProjectiles => false;
}