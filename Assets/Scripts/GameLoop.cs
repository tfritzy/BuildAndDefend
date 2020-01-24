using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public void ReturnToMap()
    {
        Map.SaveState();
        SceneChanger.ReturnToMap();
    }

    public void StartLevel()
    {
        SceneChanger.StartLevel();
    }
}