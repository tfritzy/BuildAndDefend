public static class Purchaser
{

    public static void Buy(ResourceDAO cost)
    {
        if (!CanBuy(cost))
        {
            throw new System.Exception("Not enough resources to buy this!");
        }
        Player.Data.vals.Iron -= cost.Iron;
        Player.Data.vals.Gold -= cost.Gold;
        Player.Data.vals.Wood -= cost.Wood;
        Player.Data.vals.Stone -= cost.Stone;
    }

    public static void Give(ResourceDAO amount)
    {
        Player.Data.vals.Iron += amount.Iron;
        Player.Data.vals.Gold += amount.Gold;
        Player.Data.vals.Wood += amount.Wood;
        Player.Data.vals.Stone += amount.Stone;
    }

    public static bool CanBuy(ResourceDAO cost)
    {
        if (Player.Data.vals.Iron < cost.Iron)
        {
            return false;
        }
        if (Player.Data.vals.Gold < cost.Gold)
        {
            return false;
        }
        if (Player.Data.vals.Wood < cost.Wood)
        {
            return false;
        }
        if (Player.Data.vals.Stone < cost.Stone)
        {
            return false;
        }
        return true;
    }

}