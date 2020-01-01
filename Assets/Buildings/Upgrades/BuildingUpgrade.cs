[System.Serializable]
public class BuildingUpgrade
{
    public BuildingType Type;
    public int Level;

    public void BuyUpgrade()
    {
        this.Level += 1;
    }
}