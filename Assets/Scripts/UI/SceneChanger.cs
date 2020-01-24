using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void StartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public static void ReturnToMap()
    {
        SceneManager.LoadScene("Map");
    }
}
