public class BallistaUpgrade : BuildingDAO
{
    public override ResourceDAO Cost { get => new ResourceDAO(gold: 100); }

    public BallistaUpgrade() : base(TowerType.Ballista)
    {
    }

}