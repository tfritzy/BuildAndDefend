public class GrassTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Grass;
    public override PathableType PathableType => PathableType.Pathable;
    public override bool CanBeBuiltUpon => true;
    public override bool StopsProjectiles => false;
}