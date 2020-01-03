public class BallistaUpgrade : BuildingUpgrade
{
    public override CostDAO Cost { get => new CostDAO(gold: 100); }

    public BallistaUpgrade(){
        this.Type = BuildingType.Ballista;
    }

}