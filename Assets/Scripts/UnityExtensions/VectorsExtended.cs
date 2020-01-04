using UnityEngine;

public static class Vectors_Extended
{
    public static string ToStr(this Vector2Int v)
    {
        return $"{v.x},{v.y}";
    }

    public static Vector2Int ToVector2Int(this string vectorString)
    {

        string[] vals = vectorString.Split(',');
        return new Vector2Int(int.Parse(vals[0]), int.Parse(vals[1]));
    }
}