[System.Serializable]
public class BuildingUpgrade
{
    public BuildingType Type;
    public int Level;
    public virtual CostDAO Cost { get; }

    public void BuyUpgrade()
    {
        if (Purchaser.CanBuy(this.Cost)){
            Purchaser.Buy(this.Cost);
        } else {
            // TODO Make Real Can't buy message
            UnityEngine.Debug.Log("Not enough money to buy.");
        }
        this.Level += 1;
    }

    public BuildingUpgrade GetInstance(){
        switch(this.Type){
            case (BuildingType.Ballista):
                return new BallistaUpgrade();
            default:
                return new BuildingUpgrade();
        }
    }
}