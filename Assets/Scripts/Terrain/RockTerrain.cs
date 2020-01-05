public class RockTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Rock;
    public override PathableType PathableType => PathableType.UnPathable;
    public override bool CanBeBuiltUpon => false;
}