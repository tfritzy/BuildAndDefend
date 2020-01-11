public class FireBoltUpgrade : BuildingUpgrade
{
    public override ResourceDAO Cost { get => new ResourceDAO(gold: 100); }

    public FireBoltUpgrade() : base(TowerType.FireBolt)
    {
    }

}