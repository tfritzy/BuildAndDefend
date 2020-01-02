using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridLight : MonoBehaviour {

    public float colorOfSpot;
    public GameObject imageOnSpot;

    public GridLight(GameObject image)
    {
        this.colorOfSpot = .8f;
        this.imageOnSpot = image;
        SetColor();
    }

    public void SetColor()
    {
        Color currentColor = imageOnSpot.GetComponent<Image>().color;
        currentColor.a = colorOfSpot;
        imageOnSpot.GetComponent<Image>().color = currentColor;
    }
}
