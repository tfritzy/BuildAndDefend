using Newtonsoft.Json;

[System.Serializable]
public class BuildingDAO
{
    public BuildingType Type;
    public int Level;
    public int Tier;
    public int XP;
    public int Kills;
    public double DamageDealt;

    public BuildingDAO(BuildingType type)
    {
        this.Type = type;
    }

    public void BuyLevelUp()
    {
        ResourceDAO cost = GameObjectCache.Buildings[this.Type].GetComponent<Building>().LevelUpCost;
        if (Purchaser.CanBuy(cost))
        {
            Purchaser.Buy(cost);
        }
        else
        {
            // TODO Make Real Can't buy message
            UnityEngine.Debug.Log("Not enough money to buy Level.");
        }
        this.Level += 1;
    }

    public void BuyPowerUp()
    {
        ResourceDAO cost = GameObjectCache.Buildings[this.Type].GetComponent<Building>().PowerUpCost;
        if (Purchaser.CanBuy(cost))
        {
            Purchaser.Buy(cost);
        }
        else
        {
            // TODO Make Real Can't buy message
            UnityEngine.Debug.Log("Not enough money to buy Power up.");
        }
        this.Tier += 1;
    }
}