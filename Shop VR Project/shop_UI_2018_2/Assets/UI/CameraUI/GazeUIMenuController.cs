﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeUIMenuController : MonoBehaviour {
    private MainController mainController;

	// Use this for initialization
	void Start ()
    {
        mainController = MainController.Instance();
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void ViewRotateLeft(bool x)
    {
        if (mainController.UIPointerState == 2)
        {
            mainController.movement.ViewRotateLeft(0.5f);
        }
    }

    public void ViewRotateRight(bool x)
    {
        if (mainController.UIPointerState == 2)
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
