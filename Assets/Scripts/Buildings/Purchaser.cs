public static class Purchaser {

    public static void Buy(CostDAO cost){
        if (Player.data.vals.Iron >= cost.Iron){
            Player.data.vals.Iron -= cost.Iron;
        }
        if (Player.data.vals.Gold >= cost.Gold){
            Player.data.vals.Gold -= cost.Gold;
        }
        if (Player.data.vals.Wood >= cost.Wood){
            Player.data.vals.Wood -= cost.Wood;
        }
        if (Player.data.vals.Stone >= cost.Stone){
            Player.data.vals.Stone -= cost.Stone;
        }
    }

    public static bool CanBuy(CostDAO cost){
        if (Player.data.vals.Iron < cost.Iron){
            return false;
        }
        if (Player.data.vals.Gold < cost.Gold){
            return false;
        }
        if (Player.data.vals.Wood < cost.Wood){
            return false;
        }
        if (Player.data.vals.Stone < cost.Stone){
            return false;
        }
        return true;
    }

}