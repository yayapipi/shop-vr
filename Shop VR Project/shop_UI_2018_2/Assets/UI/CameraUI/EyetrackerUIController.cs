using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackerUIController : MonoBehaviour {
    private MainController mainController;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ViewRotateLeft()
    {
        if (MainController.Instance().UIPointerState == 2)
        {
            mainController.movement.ViewRotateLeft(0.5f);
        }
    }

    public void ViewRotateRight()
    {
        if (MainController.Instance().UIPointerState == 2)
        {
            mainController.movement.ViewRotateRight(0.5f);
        }
    }

    public void Openshop()
    {
        mainController.OpenShop();
    }

    public void OpenInventory()
    {
        mainController.OpenInventory();
    }

    public void OpenSetting()
    {
        mainController.OpenSetting();
    }
}
