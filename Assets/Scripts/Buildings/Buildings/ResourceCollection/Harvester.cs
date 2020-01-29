public abstract class Harvester : Building
{
    public abstract ResourceDAO ResourceProductionPerHour { get; }

    public override void Setup()
    {
        base.Setup();
        RegisterResourceProduction();
    }

    public void RegisterResourceProduction()
    {
        if (Player.Data.ResourceHarvestByMapPerHour.ContainsKey(Player.Data.CurrentLevel) == false)
        {
            Player.Data.ResourceHarvestByMapPerHour.Add(Player.Data.CurrentLevel, new ResourceDAO());
        }

        if (!Map.Harvesters.ContainsKey(BuildingId))
        {
            Map.Harvesters.Add(BuildingId, ResourceProductionPerHour);
        }
        else
        {
            Map.Harvesters[BuildingId] = ResourceProductionPerHour;
        }
    }

    public void DeRegisterResourceProduction()
    {
        if (Player.Data.ResourceHarvestByMapPerHour.ContainsKey(Player.Data.CurrentLevel) == false)
        {
            throw new System.Exception("It shouldn't be possible that this harvester" +
                "is getting de-registered from a map that isn't in playerdata");
        }

        Map.Harvesters.Remove(BuildingId);
    }
}