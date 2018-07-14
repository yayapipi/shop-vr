using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;

	// Use this for initialization
	void Start ()
    {
        buttonPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
