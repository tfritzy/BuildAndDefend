
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

    public override string ToString()
    {
        return $"{Gold} gold, {Wood} wood, {Stone} stone, {Iron} iron, {SkillPoints} skillPoints";
    }

    public void Add(ResourceDAO addedValue)
    {
        this.Gold += addedValue.Gold;
        this.Wood += addedValue.Wood;
        this.Stone += addedValue.Stone;
        this.Iron += addedValue.Iron;
        this.SkillPoints += addedValue.SkillPoints;
    }

    public void Subtract(ResourceDAO subtractedValue)
    {
        this.Gold -= subtractedValue.Gold;
        this.Wood -= subtractedValue.Wood;
        this.Stone -= subtractedValue.Stone;
        this.Iron -= subtractedValue.Iron;
        this.SkillPoints -= subtractedValue.SkillPoints;
    }

}