using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void StartLevel(){
        SceneManager.LoadScene("Game");
    }

    public void ReturnToMap(){
        SceneManager.LoadScene("Map");
    }
}
