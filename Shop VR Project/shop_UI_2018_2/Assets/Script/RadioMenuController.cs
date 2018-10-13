using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMenuController : MonoBehaviour {
    
    public GameObject BasePanel;
    public GameObject ViewRotatePanel;
    public GameObject ModelPanel;
    public GameObject ModelScalePanel;
    public GameObject ModelRotatePanel;
    public GameObject drawModule;

    public Texture[] paint_texture = new Texture[200];
    int color_count = 0;
   // private GameObject drawModule = null;
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
            case 7:
                //open draw panel
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
        mainController.SetIsPointerSelect(false);
    }

    public void Openshop()
    {
        mainController.OpenShop();
    }

    public void OpenInventory()
    {
        mainController.OpenInventory();
    }

    public void OpenDraw()
    {
        Transform spawnPoint = mainController.cameraEye;
        Material mtl;
        
        Texture metallic_texture;
        Texture normal_map_texture;
        
        //Open Panel Color Selector
        openpanel(7);

        //Setting Color Selector Position
        drawModule.SetActive(true);
        drawModule.transform.position = new Vector3(spawnPoint.position.x, 0, spawnPoint.position.z);
        drawModule.transform.rotation = Quaternion.Euler(new Vector3(0, spawnPoint.eulerAngles.y, 0));

        //Initial Color Matetial Object
       // color_count = MainController.Instance().obj_point.GetComponent<id>().item_id;
        mtl = Instantiate(MainController.Instance().material_color, transform.position, transform.rotation);
        Texture txp = Instantiate(MainController.Instance().texture_color, transform.position, transform.rotation);
        paint_texture[color_count] = txp;
        metallic_texture = MainController.Instance().obj_point.GetComponent<Renderer>().material.GetTexture("_MainTex");
        normal_map_texture = MainController.Instance().obj_point.GetComponent<Renderer>().material.GetTexture("_BumpMap");

        //Change Shader To Color Shader And Setting Variable
        MainController.Instance().obj_point.GetComponent<MeshRenderer>().material = mtl;
        MainController.Instance().obj_point.GetComponent<Renderer>().material.SetTexture("_MainTex", paint_texture[color_count]);
        if(metallic_texture)
            MainController.Instance().obj_point.GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", metallic_texture);
        if(normal_map_texture)
            MainController.Instance().obj_point.GetComponent<Renderer>().material.SetTexture("_BumpMap", normal_map_texture);

        //Change DrawModule Texture Target
        GameObject.Find("CanvasCamera").GetComponent<Camera>().targetTexture = (RenderTexture) paint_texture[color_count];
        drawModule.GetComponentInChildren<TexturePainter>().canvasTexture = (RenderTexture) paint_texture[color_count];

        //Disable Object Grab
         MainController.Instance().obj_point.transform.parent = mainController.obj.transform;
         MainController.Instance().isPointerGrab = false;
         MainController.Instance().obj_move_color = false;
    }

    private void grip_back()
    {
        if (panel_type == 7)
        {
            drawModule.SetActive(false);
            MainController.Instance().obj_move_color = true;
            //Destroy(drawModule);
        }

        if (panel_type % 5 == 0)
        {
            if (panel_type != 0)
            {
                openpanel(0);
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
