using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler
{
    public bool buttonPressed;
    public bool buttonHover;

    [System.Serializable]
    public class eventButton
    {
        public Sprite ButtonIcon;
        public UnityEvent OnClick = new UnityEvent();
        public UnityEvent OnHold = new UnityEvent();
        public UnityEvent OnHoverEnter = new UnityEvent();
        public UnityEvent OnHoverExit = new UnityEvent();
        public UnityEvent OnHover = new UnityEvent();
    }

    public eventButton button;

    // Use this for initialization
    void Start ()
    {
        buttonPressed = false;
        buttonHover = false;
    }

    void Update()
    {
        if (buttonHover)
        {
            button.OnHover.Invoke();
        }

        if (buttonPressed)
        {
            button.OnHold.Invoke();
        }
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
        button.OnHoverEnter.Invoke();
        buttonHover = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        button.OnHoverExit.Invoke();
        buttonHover = false;
    }

    public void OnSubmit(BaseEventData pointerEventData)
    {
        button.OnClick.Invoke();
    }
}
