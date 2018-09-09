using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPointerSwitcher : MonoBehaviour {

    void Start()
    {
        GetComponent<Canvas>().worldCamera = MainController.currentPointerCamera;
    }

    void OnEnable()
    {
        MainController.UIPointerEvent += ChangeEventCamera;
    }

    void OnDisable()
    {
        MainController.UIPointerEvent -= ChangeEventCamera;
    }

    private void ChangeEventCamera(Camera eventCamera, int state)
    {
        GetComponent<Canvas>().worldCamera = eventCamera;
    }
}
