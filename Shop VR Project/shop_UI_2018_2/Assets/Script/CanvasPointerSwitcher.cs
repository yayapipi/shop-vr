using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPointerSwitcher : MonoBehaviour {

    void OnEnable()
    {
        MainController.UIPointerEvent += ChangeEventCamera;
    }

    void OnDisable()
    {
        MainController.UIPointerEvent -= ChangeEventCamera;
    }

    private void ChangeEventCamera(Camera eventCamera)
    {
        GetComponent<Canvas>().worldCamera = eventCamera;
    }
}
