public class SandTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Sand;
    public override PathableType PathableType => PathableType.Pathable;
    public override bool CanBeBuiltUpon => true;
}