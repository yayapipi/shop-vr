using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMenuController : MonoBehaviour {
    
    public GameObject BasePanel_0;
    public GameObject ModelPanel_1;
    public GameObject SpecialPanel_2;
    public GameObject ViewRotatePanel_10;
    public GameObject GrabPanel_11;
    public GameObject DrawPanel_12;
    //public GameObject ModelScalePanel;
    //public GameObject ModelRotatePanel;
    private DrawModule drawModule;
    private MainController mainController;
    private static int panel_type = 0;
    private static int panel_back = 0;

    public GameObject modelSetting;
    private GameObject model_SettingObj;
    public GameObject systemSetting;

    /*
    0 base panel    1 shop panel    2 view rotate   3 setting panel     4 backpack panel 
    5 Model panel   6 scale panel   7 draw panel    8 putback panel     9 rotate panel
    */
    void OnEnable()
    {
        MainController.RGripClickDown += grip_back;
    }

    void OnDisable()
    {
        MainController.RGripClickDown -= grip_back;
    }

    // Use this for initialization
    void Start () 
    {
        BasePanel_0.SetActive(true);
        ModelPanel_1.SetActive(false);
        SpecialPanel_2.SetActive(false);
        ViewRotatePanel_10.SetActive(false);
        GrabPanel_11.SetActive(false);
        DrawPanel_12.SetActive(false);
        mainController = MainController.Instance();
        drawModule = mainController.drawModule.GetComponent<DrawModule>();
    }
	
	// Update is called once per frame
	void Update () 
    {

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
                case 10:
                    MainController.isViewRotate = false;
                    break;
                case 11:
                    MainController.isViewRotate = false;
                    break;
                case 12:
                    //Disable Color Painter
                    drawModule.SaveAndDisable();

                    //Enable Object Grab
                    mainController.enablePointerGrab = true;
                    break;
            }

            //open jud feature
            switch (jud)
            {
                case 0:
                    if (panel_type == 1)
                    {
                        mainController.modelMenu.DisableModelMenu();
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 10:
                    MainController.isViewRotate = true;
                    mainController.blade.SetActive(false);
                    break;
                case 11:
                    MainController.isViewRotate = true;
                    break;
                case 12:
                    //Disable Object Grab
                    mainController.enablePointerGrab = false;

                    //DeGrab
                    if (mainController.GetIsPointerGrab() && mainController.obj_point != null)
                    {
                        mainController.obj_point.transform.parent = mainController.obj.transform;
                        mainController.SetIsPointerGrab(false);
                    }

                    //Enable Color Painter
                    drawModule.Enable();
                    break;
            }
        }

        //switch panel
        switch (jud) {
            case 0:
                //open base panel
                BasePanel_0.SetActive(true);
                ModelPanel_1.SetActive(false);
                SpecialPanel_2.SetActive(false);
                ViewRotatePanel_10.SetActive(false);
                GrabPanel_11.SetActive(false);
                DrawPanel_12.SetActive(false);
                BasePanel_0.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                panel_back = 0;
                break;
            case 1:
                //open model panel
                BasePanel_0.SetActive(false);
                ModelPanel_1.SetActive(true);
                SpecialPanel_2.SetActive(false);
                ViewRotatePanel_10.SetActive(false);
                GrabPanel_11.SetActive(false);
                DrawPanel_12.SetActive(false);
                ModelPanel_1.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                panel_back = 0;
                break;
            case 2:
                //open special panel
                BasePanel_0.SetActive(false);
                ModelPanel_1.SetActive(false);
                SpecialPanel_2.SetActive(true);
                ViewRotatePanel_10.SetActive(false);
                GrabPanel_11.SetActive(false);
                DrawPanel_12.SetActive(false);
                SpecialPanel_2.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                panel_back = 1;
                break;
            case 10:
                //open view rotate panel
                BasePanel_0.SetActive(false);
                ModelPanel_1.SetActive(false);
                SpecialPanel_2.SetActive(false);
                ViewRotatePanel_10.SetActive(true);
                GrabPanel_11.SetActive(false);
                DrawPanel_12.SetActive(false);
                ViewRotatePanel_10.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 11:
                //open grab panel
                BasePanel_0.SetActive(false);
                ModelPanel_1.SetActive(false);
                SpecialPanel_2.SetActive(false);
                ViewRotatePanel_10.SetActive(false);
                GrabPanel_11.SetActive(true);
                DrawPanel_12.SetActive(false);
                GrabPanel_11.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 12:
                //open draw panel
                BasePanel_0.SetActive(false);
                ModelPanel_1.SetActive(false);
                SpecialPanel_2.SetActive(false);
                ViewRotatePanel_10.SetActive(false);
                GrabPanel_11.SetActive(false);
                DrawPanel_12.SetActive(true);
                DrawPanel_12.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            default:
                Debug.Log("error radio panel index");
                isOpenPanel = false;
                break;
        }

        if (isOpenPanel)
            panel_type = jud;
    }

    public void openModelSetting()
    {
        mainController.modelMenu.SwitchModelMenu();
    }


    public void SelectObj()
    {
        if (panel_type != 10)
        {
            openpanel(1);
        }
        else
        {
            SetPanelBack(1);
        }
    }

    public void DeSelectObj()
    {
        openpanel(panel_back);
    }

    public void GrabObj()
    {
        if (!mainController.blade.activeSelf)
        {
            SetPanelBack(panel_type);
            openpanel(11);
        }
    }

    public void DeGrabObj()
    {
        if (!mainController.blade.activeSelf)
        {
            if (panel_back == 10)
            {
                grip_back();
                SetPanelBack(1);
            }
            else if (panel_back > 10)
            {
                throw new Exception("Error code: 1");
            }
            else
            {
                grip_back();
            }
        }
    }

    public static int getPanelType()
    {
        return panel_type;
    }

    public void SetPanelBack(int jud)
    {
        panel_back = jud;
    }

    public void put_back()
    {
        openpanel(0);
        ShopController.PutBack(mainController.obj_point.GetComponent<id>().item_id);
        Destroy(mainController.obj_point);
        mainController.SetIsPointerSelect(false);
        mainController.SetIsPointerGrab(false);
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

    public void OpenDraw()
    {
        //Open Panel Color Selector
        openpanel(12);
    }

    public void grip_back()
    {
        if (panel_type != 1)
        {
            openpanel(panel_back);
        }
    }

    public void slice_model()
    {
        openpanel(10);
        panel_back = 2;
        mainController.blade.SetActive(true);
    }

    public void object_Depth(int speed)
    {
        if (mainController.GetIsPointerSelect())
        {
            float step = speed * Time.deltaTime;
            mainController.obj_point.transform.position = Vector3.MoveTowards(mainController.obj_point.transform.position, mainController.rightController.transform.position, step);
        }
    }

    public void auto_alignment()
    {
        if (mainController.obj_select == 0)
        {
            mainController.descripUI.StartDescription(12);
            mainController.obj_select = 1;
            mainController.enablePointerSelectOtherObject = true;
            mainController.obj_select1 = null;
            mainController.obj_select2 = null;
        }
    }

    public void scale_obj(int btn)
    {
        //btn 1 - up 2 - down
        switch (btn)
        {
            case 1:
                if (mainController.GetIsPointerSelect())
                {
                    mainController.obj_point.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
                }
                break;
            case 2:
                if (mainController.GetIsPointerSelect())
                {
                    mainController.obj_point.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
                }
                break;
        };
    }

    public void rotate_obj(int btn)
    {
        //btn 1 - up 2 - down 3-left 4 -right
        switch (btn)
        {
            case 1:
                if (mainController.GetIsPointerSelect())
                {
                    mainController.obj_point.transform.localEulerAngles += new Vector3(25f, 0f, 0) * Time.deltaTime;
                }
                break;
            case 2:
                if (mainController.GetIsPointerSelect())
                {
                    mainController.obj_point.transform.localEulerAngles -= new Vector3(25f,0f, 0) * Time.deltaTime;
                }
                break;
            case 3:
                if (mainController.GetIsPointerSelect())
                {
                    mainController.obj_point.transform.localEulerAngles += new Vector3(0, 25f, 0) * Time.deltaTime;
                }
                break;
            case 4:
                if (mainController.GetIsPointerSelect())
                {
                    mainController.obj_point.transform.localEulerAngles -= new Vector3(0, 25f, 0) * Time.deltaTime;
                }
                break;
        }
    }
    
}
