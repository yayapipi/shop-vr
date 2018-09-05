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

    protected int panel_type = 1; //1-View_Rotate 2-Model_View

    // Use this for initialization
    void Start () {
        BasePanel.SetActive(true);
        ViewRotatePanel.SetActive(false);
        ModelPanel.SetActive(false);
        ModelScalePanel.SetActive(false);
        ModelRotatePanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        grip_back();
	}

    public void openpanel(int jud) {
        switch (jud) {
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
                //open base panel
                BasePanel.SetActive(true);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                BasePanel.transform.GetComponent<VRTK.VRTK_RadialMenu>().ShowMenu();
                break;
        }
    }

    public void put_back() {
      //  ShopController.PutBack(MainController.obj_point.GetComponent<id>().item_id);
        Destroy(MainController.obj_point);
    }

    public void scalebtn()
    {
        MainController.isScale = true;
        MainController.isRotate = false;
        MainController.isViewRotate = false;
    }

    public void Openshop()
    {
        MainController.Instance().OpenShop();
    }
    public void rotatebtn()
    {
        MainController.isScale = false;
        MainController.isRotate = true;
        MainController.isViewRotate = false;
    }

    public void OpenInventory()
    {
        MainController.Instance().OpenInventory();
    }

    public void ViewRotate()
    {
        MainController.isViewRotate = true;
        //Call Controller UI
    }
    public void grip_back()
    {
        if (rcontroller.GetComponent<SteamVR_TrackedController>().gripped)
        {
            if (panel_type == 2)
            {
                openpanel(2); // Open Model Panel
            }
            else if (panel_type == 1)
            {
                openpanel(0); // Open Default Panel
            }
        }
    }

    public void scale_obj(int btn)
    {
        //btn 1 - up 2 - down
        switch (btn)
        {
            case 1:
                if (MainController.GetIsSelect())
                {
                     if (MainController.isScale)
                     {
                        MainController.obj_point.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
                    }
                }
                break;
            case 2:
                if (MainController.GetIsSelect())
                {
                    if (MainController.isScale)
                    {
                        MainController.obj_point.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
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
                if (MainController.GetIsSelect())
                {
                    if (MainController.isRotate)
                    {
                        MainController.obj_point.transform.localEulerAngles += new Vector3(25f, 0f, 0) * Time.deltaTime;
                    }
                }
                break;
            case 2:
                if (MainController.GetIsSelect())
                {
                    if (MainController.isRotate)
                    {
                        MainController.obj_point.transform.localEulerAngles -= new Vector3(25f,0f, 0) * Time.deltaTime;
                    }
                }
                break;
            case 3:
                if (MainController.GetIsSelect())
                {
                    if (MainController.isRotate)
                    {
                        MainController.obj_point.transform.localEulerAngles += new Vector3(0, 25f, 0) * Time.deltaTime;
                    }
                }
                break;
            case 4:
                if (MainController.GetIsSelect())
                {
                    if (MainController.isRotate)
                    {
                        MainController.obj_point.transform.localEulerAngles -= new Vector3(0, 25f, 0) * Time.deltaTime;
                    }
                }
                break;
        };
    }
    
}
