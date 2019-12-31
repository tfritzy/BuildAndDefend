using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStarter : MonoBehaviour
{
    public void StartLevel(){
        SceneManager.LoadScene("Game");
    }
}
