using System;

public class BrushTerrain : EnvironmentTile
{
    public override EnvironmentTileType Type => EnvironmentTileType.Brush;
    public override PathableType PathableType => PathableType.UnPathable;
    public override bool CanBeBuiltUpon => false;
}