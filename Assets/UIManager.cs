using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject canvas;
    void Start(){
        this.canvas = GameObject.Find("Canvas");
    }

    private const string upgradeMenuPath = "Gameobjects/UI/KineticUpgradeMenu";
    public void OpenUpgradeMenu(){
        GameObject upgradeWindow = Resources.Load<GameObject>(upgradeMenuPath);
        Instantiate(upgradeWindow, Vector3.zero, new Quaternion(), this.canvas.transform);
    }
}
