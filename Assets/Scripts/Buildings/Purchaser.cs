public static class Purchaser
{

    public static void Buy(ResourceDAO cost)
    {
        if (!CanBuy(cost))
        {
            throw new System.Exception("Not enough resources to buy this!");
        }
        Player.data.vals.Iron -= cost.Iron;
        Player.data.vals.Gold -= cost.Gold;
        Player.data.vals.Wood -= cost.Wood;
        Player.data.vals.Stone -= cost.Stone;
    }

    public static void Give(ResourceDAO amount)
    {
        Player.data.vals.Iron += amount.Iron;
        Player.data.vals.Gold += amount.Gold;
        Player.data.vals.Wood += amount.Wood;
        Player.data.vals.Stone += amount.Stone;
    }

    public static bool CanBuy(ResourceDAO cost)
    {
        if (Player.data.vals.Iron < cost.Iron)
        {
            return false;
        }
        if (Player.data.vals.Gold < cost.Gold)
        {
            return false;
        }
        if (Player.data.vals.Wood < cost.Wood)
        {
            return false;
        }
        if (Player.data.vals.Stone < cost.Stone)
        {
            return false;
        }
        return true;
    }

}