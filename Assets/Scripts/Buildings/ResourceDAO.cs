
[System.Serializable]
public class ResourceDAO
{
    public ResourceDAO(int gold = 0, int wood = 0, int stone = 0, int iron = 0, int skillPoints = 0)
    {
        this.Gold = gold;
        this.Wood = wood;
        this.Stone = stone;
        this.Iron = iron;
        this.SkillPoints = skillPoints;
    }

    public int Gold;
    public int Wood;
    public int Stone;
    public int Iron;
    public int SkillPoints;
}