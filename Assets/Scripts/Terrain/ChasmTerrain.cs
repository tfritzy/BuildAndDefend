public class ChasmTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Chasm;
    public override PathableType PathableType => PathableType.UnPathable;
    public override bool CanBeBuiltUpon => false;
    public override bool StopsProjectiles => false;
}