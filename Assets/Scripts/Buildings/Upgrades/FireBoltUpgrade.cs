public class FireBoltUpgrade : BuildingDAO
{
    public override ResourceDAO Cost { get => new ResourceDAO(gold: 100); }

    public FireBoltUpgrade() : base(TowerType.FireBolt)
    {
    }

}