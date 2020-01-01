using System.Collections.Generic;

[System.Serializable]
public class PlayerDataDAO
{
    public int gold;
    public int iron;
    public int wood;
    public int stone;

    public List<string> beatenLevels;

    public string currentLevel;
}