public static class Purchaser
{

    public static void Buy(ResourceDAO cost)
    {
        if (!CanBuy(cost))
        {
            throw new System.Exception("Not enough resources to buy this!");
        }
        Player.PlayerData.Values.Iron -= cost.Iron;
        Player.PlayerData.Values.Gold -= cost.Gold;
        Player.PlayerData.Values.Wood -= cost.Wood;
        Player.PlayerData.Values.Stone -= cost.Stone;
        Player.PlayerData.Values.SkillPoints -= cost.SkillPoints;
    }

    public static void Give(ResourceDAO amount)
    {
        Player.PlayerData.Values.Iron += amount.Iron;
        Player.PlayerData.Values.Gold += amount.Gold;
        Player.PlayerData.Values.Wood += amount.Wood;
        Player.PlayerData.Values.Stone += amount.Stone;
        Player.PlayerData.Values.SkillPoints += amount.SkillPoints;
    }

    public static bool CanBuy(ResourceDAO cost)
    {
        if (Player.PlayerData.Values.Iron < cost.Iron)
        {
            return false;
        }
        if (Player.PlayerData.Values.Gold < cost.Gold)
        {
            return false;
        }
        if (Player.PlayerData.Values.Wood < cost.Wood)
        {
            return false;
        }
        if (Player.PlayerData.Values.Stone < cost.Stone)
        {
            return false;
        }
        if (Player.PlayerData.Values.SkillPoints < cost.SkillPoints)
        {
            return false;
        }

        return true;
    }

}