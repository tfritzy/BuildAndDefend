using Newtonsoft.Json;

[System.Serializable]
public class BuildingDAO
{
    public TowerType Type;
    public int Level;
    public virtual ResourceDAO Cost { get; }
    public int XP;
    public int Kills;
    public double DamageDealt;
    public string Name;

    public BuildingDAO(TowerType type)
    {
        this.Type = type;
    }

    public void BuyUpgrade()
    {
        if (Purchaser.CanBuy(this.Cost))
        {
            Purchaser.Buy(this.Cost);
        }
        else
        {
            // TODO Make Real Can't buy message
            UnityEngine.Debug.Log("Not enough money to buy.");
        }
        this.Level += 1;
    }

    public BuildingDAO GetInstance()
    {
        var serializedUpgrade = JsonConvert.SerializeObject(this);
        switch (this.Type)
        {
            case (TowerType.Ballista):
                return JsonConvert.DeserializeObject<BallistaUpgrade>(serializedUpgrade);
            default:
                return this;
        }
    }
}