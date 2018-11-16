using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeUIMenuController : MonoBehaviour {
    public GameObject panel_1;
    public GameObject panel_2;
    private Image[] buttons;
    private MainController mainController;
    private static int panel_type = 0;
    private static GazeUIMenuController _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static GazeUIMenuController Instance()
    {
        if (_instance == null)
        {
            throw new Exception(" could not find the GazeUIMenuController object.");
        }
        return _instance;
    }

    void OnDestroy()
    {
        _instance = null;
    }

	void Start ()
    {
        mainController = MainController.Instance("GazeUIMenuController");
        panel_type = 0;
        panel_1.SetActive(false);
        panel_2.SetActive(false);

        buttons = new Image[9];
        for (int i = 0; i < 3; i++)
        {
            buttons[i] = panel_1.transform.GetChild(i).GetComponent<Image>();
        }

        for (int i = 3; i < 9; i++)
        {
            buttons[i] = panel_1.transform.GetChild(i-3).GetComponent<Image>();
        }
    }

    public void openpanel(int jud)
    {
        bool isOpenPanel = true;

        if (panel_type != jud)
        {
            //close panel_type feature
            switch (panel_type)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }

            //open jud feature
            switch (jud)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        //switch panel
        switch (jud)
        {
            case 0:
                //close all panel
                panel_1.SetActive(false);
                panel_2.SetActive(false);
                break;
            case 1:
                //open base panel
                panel_1.SetActive(true);
                panel_2.SetActive(false);
                break;
            case 2:
                //open model panel
                panel_1.SetActive(false);
                panel_2.SetActive(true);
                break;
            default:
                Debug.Log("error gaze panel index");
                isOpenPanel = false;
                break;
        }

        if (isOpenPanel)
            panel_type = jud;
    }

    public static int getPanelType()
    {
        return panel_type;
    }

    public void SelectObj()
    {
    }

    public void DeSelectObj()
    {
    }

    public void GrabObj()
    {
    }

    public void DeGrabObj()
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
        if (mainController.UIPointerState == 2)
        {
            mainController.OpenShop();
        }
    }

    public void OpenInventory()
    {
        if (mainController.UIPointerState == 2)
        {
            mainController.OpenInventory();
        }
    }

    public void OpenSetting()
    {
        if (mainController.UIPointerState == 2)
        {
            mainController.OpenSetting();
        }
    }

    public void openModelSetting()
    {
        if (mainController.UIPointerState == 2)
        {
            mainController.modelMenu.SwitchModelMenu();
        }
    }

    public void put_back()
    {
        if (mainController.UIPointerState == 2)
        {
            openpanel(1);
            ShopController.PutBack(mainController.obj_point.GetComponent<id>().item_id);
            Destroy(mainController.obj_point);
            mainController.SetIsPointerSelect(false);
            mainController.SetIsPointerGrab(false);
        }
    }

    public void SwitchPanel()
    {
        if (panel_type != 0)
        {
            openpanel(0);
        }
        else if (!mainController.GetIsPointerSelect())
        {
            openpanel(1);
        }
        else
        {
            openpanel(2);
        }
    }
}
