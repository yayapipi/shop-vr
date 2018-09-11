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

    protected int panel_type = 1; //1-View_Rotate 2-Model_View

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
            case 1:
                //open view rotate panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(true);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                ViewRotatePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                panel_type = 1;
                break;
            case 2:
                //open model panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(true);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                ModelPanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
            case 3:
                //open model scale panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(true);
                ModelRotatePanel.SetActive(false);
                ModelScalePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                panel_type = 2;
                break;
            case 4:
                //open model rotate panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(true);
                ModelRotatePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                panel_type = 2;
                break;
            default:
                Debug.Log("error radio panel index");
                break;
        }
    }

    public void put_back() {
        ShopController.PutBack(mainController.obj_point.GetComponent<id>().item_id);
        openpanel(0);
        Destroy(mainController.obj_point);
        mainController.SetIsSelect(false);
    }

    public void scalebtn()
    {
        mainController.isScale = true;
        mainController.isRotate = false;
        mainController.isViewRotate = false;
    }

    public void Openshop()
    {
        mainController.OpenShop();
    }
    public void rotatebtn()
    {
        mainController.isScale = false;
        mainController.isRotate = true;
        mainController.isViewRotate = false;
    }

    public void OpenInventory()
    {
        mainController.OpenInventory();
    }

    public void ViewRotate()
    {
        mainController.isViewRotate = true;
    }
    private void grip_back()
    {
        if (panel_type == 2)
        {
            openpanel(2); // Open Model Panel
        }
        else if (panel_type == 1)
        {
            openpanel(0); // Open Default Panel
            mainController.isViewRotate = false;
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
                     if (mainController.isScale)
                     {
                        mainController.obj_point.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
                    }
                }
                break;
            case 2:
                if (mainController.GetIsSelect())
                {
                    if (mainController.isScale)
                    {
                        mainController.obj_point.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
                    }
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
                    if (mainController.isRotate)
                    {
                        mainController.obj_point.transform.localEulerAngles += new Vector3(25f, 0f, 0) * Time.deltaTime;
                    }
                }
                break;
            case 2:
                if (mainController.GetIsSelect())
                {
                    if (mainController.isRotate)
                    {
                        mainController.obj_point.transform.localEulerAngles -= new Vector3(25f,0f, 0) * Time.deltaTime;
                    }
                }
                break;
            case 3:
                if (mainController.GetIsSelect())
                {
                    if (mainController.isRotate)
                    {
                        mainController.obj_point.transform.localEulerAngles += new Vector3(0, 25f, 0) * Time.deltaTime;
                    }
                }
                break;
            case 4:
                if (mainController.GetIsSelect())
                {
                    if (mainController.isRotate)
                    {
                        mainController.obj_point.transform.localEulerAngles -= new Vector3(0, 25f, 0) * Time.deltaTime;
                    }
                }
                break;
        }
    }
    
}
