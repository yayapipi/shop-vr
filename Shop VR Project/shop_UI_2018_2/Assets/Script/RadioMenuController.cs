using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMenuController : MonoBehaviour {
    
    public GameObject BasePanel;
    public GameObject ViewRotatePanel;
    public GameObject ModelPanel;
    public GameObject ModelScalePanel;
    public GameObject ModelRotatePanel;
    public GameObject rcontroller;
    private MainController mainController;

    private static int panel_type = 0;
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
    void Start () {
        BasePanel.SetActive(true);
        ViewRotatePanel.SetActive(false);
        ModelPanel.SetActive(false);
        ModelScalePanel.SetActive(false);
        ModelRotatePanel.SetActive(false);
        mainController = MainController.Instance();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void openpanel(int jud) {
        bool isOpenPanel = true;

        if (jud == 2)
        {
            MainController.isViewRotate = true;
        }
        else
        {
            MainController.isViewRotate = false;
        }

        switch (jud) {
            case 0:
                //open base panel
                Debug.Log("open base");
                BasePanel.SetActive(true);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                BasePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 2:
                //open view rotate panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(true);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                ViewRotatePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 5:
                //open model panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(true);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                ModelPanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 6:
                //open model scale panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(true);
                ModelRotatePanel.SetActive(false);
                ModelScalePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 9:
                //open model rotate panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(true);
                ModelRotatePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            default:
                Debug.Log("error radio panel index");
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

    public void put_back() {
        ShopController.PutBack(mainController.obj_point.GetComponent<id>().item_id);
        openpanel(0);
        Destroy(mainController.obj_point);
        mainController.SetIsSelect(false);
    }

    public void Openshop()
    {
        mainController.OpenShop();
    }

    public void OpenInventory()
    {
        mainController.OpenInventory();
    }

    private void grip_back()
    {
        if (panel_type % 5 == 0)
        {
            if (panel_type != 0)
            {
                openpanel(panel_type - 5);
            }
        }
        else
        {
            openpanel(panel_type - panel_type % 5);
        }
    }

    public void scale_obj(int btn)
    {
        //btn 1 - up 2 - down
        switch (btn)
        {
            case 1:
                if (mainController.GetIsSelect())
                {
                    mainController.obj_point.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
                }
                break;
            case 2:
                if (mainController.GetIsSelect())
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
                if (mainController.GetIsSelect())
                {
                    mainController.obj_point.transform.localEulerAngles += new Vector3(25f, 0f, 0) * Time.deltaTime;
                }
                break;
            case 2:
                if (mainController.GetIsSelect())
                {
                    mainController.obj_point.transform.localEulerAngles -= new Vector3(25f,0f, 0) * Time.deltaTime;
                }
                break;
            case 3:
                if (mainController.GetIsSelect())
                {
                    mainController.obj_point.transform.localEulerAngles += new Vector3(0, 25f, 0) * Time.deltaTime;
                }
                break;
            case 4:
                if (mainController.GetIsSelect())
                {
                    mainController.obj_point.transform.localEulerAngles -= new Vector3(0, 25f, 0) * Time.deltaTime;
                }
                break;
        }
    }
    
}
