using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseUIWindows : MonoBehaviour, IPointerDownHandler, IBeginDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        CloseAllUIWindows();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CloseAllUIWindows();
    }

    public void CloseAllUIWindows()
    {
        GameObject[] windows = GameObject.FindGameObjectsWithTag("UIWindow");
        foreach (GameObject window in windows){
            Destroy(window.gameObject);
        }
    }
}
