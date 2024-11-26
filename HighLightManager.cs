using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighLightManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // This will be called when the mouse enters the UI element (like a button or panel)
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered the UI element!");
        // You can add your code here to change UI element properties on mouse enter

        GetComponent<Image>().enabled = true;
    }

    // This will be called when the mouse exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited the UI element!");
        // You can add your code here to revert changes when the mouse exits

        GetComponent<Image>().enabled = false;
    }
}