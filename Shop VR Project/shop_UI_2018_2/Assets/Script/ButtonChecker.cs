using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool buttonPressed;
    public bool buttonHover;

	// Use this for initialization
	void Start ()
    {
        buttonPressed = false;
        buttonHover = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        buttonHover = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        buttonHover = false;
    }
}
