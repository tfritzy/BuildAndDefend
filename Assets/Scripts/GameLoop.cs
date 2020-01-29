using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public void ReturnToMap()
    {
        Map.SaveState();
        Player.Save();
        SceneChanger.ReturnToMap();
    }

    public void StartLevel()
    {
        SceneChanger.StartLevel();
    }
}