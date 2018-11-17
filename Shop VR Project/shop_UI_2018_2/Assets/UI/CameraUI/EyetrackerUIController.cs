using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyetrackerUIController : MonoBehaviour
{
    public GameObject panel_1;
    public GameObject panel_2;
    public GameObject back;
    public GameObject panelSwitch;
    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bot;
    public GameObject altButtons;
    private Image[] buttons;
    private MainController mainController;
    private static int panel_type = 0;
    private static EyetrackerUIController _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static EyetrackerUIController Instance()
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

    void OnEnable()
    {
        MainController.UIPointerEvent += ChangeState;
    }

    void OnDisable()
    {
        MainController.UIPointerEvent -= ChangeState;
    }

    private void ChangeState(Camera eventCamera)
    {
        if(mainController.UIPointerState != 2)
        {
            Disable();
        }
        else
        {
            Enable();
        }
    }

    private void Disable()
    {
        openpanel(0);
        back.SetActive(false);
        panelSwitch.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        altButtons.SetActive(false);
    }

    private void Enable()
    {
        openpanel(0);
        back.SetActive(true);
        panelSwitch.SetActive(true);
        left.SetActive(true);
        right.SetActive(true);
        altButtons.SetActive(true);
    }

    void Start()
    {
        mainController = MainController.Instance("EyetrackerUIController");
        panel_type = 0;
        panel_1.SetActive(false);
        panel_2.SetActive(false);

        buttons = new Image[9];
        for (int i = 0; i < 3; i++)
        {
            buttons[i] = panel_1.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }

        for (int i = 3; i < 9; i++)
        {
            buttons[i] = panel_2.transform.GetChild(i - 3).GetChild(0).GetComponent<Image>();
        }

        ChangeState(null);
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
                    back.SetActive(false);
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
                    back.SetActive(true);
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

    public void Highlight(int button_index)
    {
        if (button_index < 10)
        {
            buttons[button_index].color = new Color(0.5f, 0.5f, 0.5f, buttons[button_index].color.a);
        }
    }

    public void DeHighlight(int button_index)
    {
        if (button_index < 10)
        {
            buttons[button_index].color = new Color(1f, 1f, 1f, buttons[button_index].color.a);
        }
    }

    public void SelectObj()
    {
    }

    public void DeSelectObj()
    {
        mainController.modelMenu.DisableModelMenu();
    }

    public void GrabObj()
    {
    }

    public void DeGrabObj()
    {
    }

    public void ViewRotateLeft()
    {
        if (mainController.UIPointerState == 2)
        {
            mainController.movement.ViewRotateLeft(0.5f);
        }
    }

    public void ViewRotateRight()
    {
        if (mainController.UIPointerState == 2)
        {
            mainController.movement.ViewRotateRight(0.5f);
        }
    }

    public void Openshop()
    {
        if (mainController.UIPointerState == 2 && panel_type != 0)
        {
            openpanel(0);
            mainController.OpenShop();
        }
    }

    public void OpenInventory()
    {
        if (mainController.UIPointerState == 2 && panel_type != 0)
        {
            openpanel(0);
            mainController.OpenInventory();
        }
    }

    public void OpenSetting()
    {
        if (mainController.UIPointerState == 2 && panel_type != 0)
        {
            openpanel(0);
            mainController.OpenSetting();
        }
    }

    public void openModelSetting()
    {
        if (mainController.UIPointerState == 2 && panel_type == 2)
        {
            openpanel(0);
            mainController.modelMenu.SwitchModelMenu();
        }
    }

    public void put_back()
    {
        if (mainController.UIPointerState == 2 && panel_type == 2)
        {
            openpanel(0);
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

    //DeGrab and DeSelect
    public void Back()
    {
        if(panel_type == 0)
        {
            GameGaze.StartEyeBackEvent();
        }
    }
}
