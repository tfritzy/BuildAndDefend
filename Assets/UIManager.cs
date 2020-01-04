using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject canvas;
    void Start(){
        this.canvas = GameObject.Find("Canvas");
    }

    public void OpenUpgradeMenu(){
        GameObject upgradeWindow = Resources.Load<GameObject>($"{FilePaths.Buildings}/KineticUpgradeMenu");
        Instantiate(upgradeWindow, Vector3.zero, new Quaternion(), this.canvas.transform);
    }
}
