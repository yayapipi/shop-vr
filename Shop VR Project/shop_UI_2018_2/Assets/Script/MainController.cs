using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MainController : MonoBehaviour {
    [Header("Related Objects")]
    public GameObject shopMain;
    public GameObject inventoryMain;
    public GameObject systemSetting;
    public RadioMenuController radioMenu;
    public DescriptionUIManager descripUI;
    public Movement movement;
    public Transform cameraEye;
    public VRTK.VRTK_ControllerEvents leftController;
    public VRTK.VRTK_ControllerEvents rightController;
    public Camera ControllerPointerCamera;
    public Camera EyetrackerPointerCamera;
    public Camera KeyboardPointerCamera;
    public static Camera currentPointerCamera;
    public LayerMask gizmoLayer;
    public Camera DisplayCamera;

    public GameObject obj;
    private static int userID;
    private static sqlapi sqlConnection;
    private int isOpenUI;

    [Header("Variables")]
    public GameObject obj_point = null;
    public Transform trackPoint;
    public bool grabMode;
    public bool obj_isKinematic;
    public bool obj_useGravity;
    public bool enablePointerSelect = true;
    public bool enablePointerGrab = true;
    public bool enablePointerCutMesh = true;
    private bool isPointerSelect = false;
    private bool isPointerGrab = false;
    public static bool isViewRotate = false;
    private static MainController _instance = null;
    public float scaleLimit = 0.5f;

    [Header("RayInformation")]
    public Vector3 rayHitPos;

    [Header("Select Other Object")]
    public bool enablePointerSelectOtherObject = false;
    public int condition = 0;
    public GameObject obj_select1 = null;
    public GameObject obj_select2 = null;
    public int obj_select = 0;

    [Header("Tools")]
    public GameObject drawModule;
    public GameObject gizmos;
    public GameObject blade;
    public ModelMenuController modelMenu;

    //Controller state
    [Header("Controller States")]
    public bool RTriggerTouch = false;
    public bool RTriggerPress = false;
    public bool RTriggerClick = false;
    public bool RTriggerClickDown_bool = false;
    public bool RTriggerClickUp_bool = false;
    private bool RTriggerClickLast = false;
    public bool RGripTouch = false;
    public bool RGripPress = false;
    public bool RGripClick = false;

    //Controller events
    public delegate void ControllerEventManager();
    public static event ControllerEventManager RTriggerClickDown;
    public static event ControllerEventManager RTriggerClickUp;
    public static event ControllerEventManager RTriggerPressDown;
    public static event ControllerEventManager RGripClickDown;

    //Canvas UI pointer event
    public delegate void CanvasUIPointerEventManager(Camera eventCamera);
    public static event CanvasUIPointerEventManager UIPointerEvent;
    public int UIPointerState = 1;
    public int LastUIPointerState = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        //Set Canvas event camera
        switch (UIPointerState)
        {
            case 0:
                currentPointerCamera = null;
                break;
            case 1:
                currentPointerCamera = ControllerPointerCamera;
                break;
            case 2:
                currentPointerCamera = EyetrackerPointerCamera;
                break;
            case 3:
                currentPointerCamera = KeyboardPointerCamera;
                break;
        }

        isOpenUI = 0;
        sqlConnection = new sqlapi();
        userID = 1;

        //Find gameobject
        obj = GameObject.Find("Object");
        if (!obj)
        {
            obj = new GameObject();
        }
    }

    void OnEnable()
    {
        if(rightController)
        {
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, true, DoRTriggerTouched);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, false, DoRTriggerUnTouched);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, DoRTriggerPressed);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, false, DoRTriggerUnPressed);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, true, DoRTriggerClicked);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, false, DoRTriggerUnClicked);

            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, true, DoRGripTouched);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, false, DoRGripUnTouched);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, true, DoRGripPressed);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, false, DoRGripUnPressed);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, true, DoRGripClicked);
            rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, false, DoRGripUnClicked);

            leftController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, true, DoLGripClicked);
        }
    }

    void OnDisable()
    {
        if(rightController)
        {
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, true, DoRTriggerTouched);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, false, DoRTriggerUnTouched);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, DoRTriggerPressed);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, false, DoRTriggerUnPressed);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, true, DoRTriggerClicked);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, false, DoRTriggerUnClicked);

            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, true, DoRGripTouched);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, false, DoRGripUnTouched);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, true, DoRGripPressed);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, false, DoRGripUnPressed);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, true, DoRGripClicked);
            rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, false, DoRGripUnClicked);

            leftController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, true, DoLGripClicked);
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

    // Use this for initialization
    void Start () {
        //Update user information
        UpdateUserData();
        if (DisplayCamera)
        {
            StartCoroutine(Display());
        }
    }

    public IEnumerator Display()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Disable");
        DisplayCamera.enabled = false;
        yield return new WaitForSeconds(1f);
        Debug.Log("Enable");
        DisplayCamera.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        RTriggerClickDown_bool = RTriggerClick && !RTriggerClickLast;
        RTriggerClickUp_bool = !RTriggerClick && RTriggerClickLast;
        RTriggerClickLast = RTriggerClick;
	}

    void OnApplicationQuit()
    {
        sqlConnection.closeSql();
    }

    public static MainController Instance(String g)
    {
        if (_instance == null)
        {
            throw new Exception( g + " could not find the MainController object.");
        }
        return _instance;
    }

    public static sqlapi getSqlConnection()
    {
        return sqlConnection;
    }

    public static int GetUserID()
    {
        return userID;
    }

    public void OpenShop()
    {
        StartCoroutine(SwitchUI(1));
    }

    public void OpenInventory()
    {
        StartCoroutine(SwitchUI(2));
    }

    public void OpenSetting()
    {
        StartCoroutine(SwitchUI(3));
    }

    public void CloseAllUI()
    {
        StartCoroutine(SwitchUI(0));
    }

    private IEnumerator SwitchUI(int index)
    {
        if (isOpenUI != index)
        {
            if (isOpenUI == 1)
            {
                ShopController.Instance().Close();
            }
            else if (isOpenUI == 2)
            {
                InventoryController.Instance().Close();
            }
            else if (isOpenUI == 3)
            {
                SettingController.Instance().Close();
            }

            isOpenUI = index;

            yield return new WaitForSeconds(0.1f);

            switch (index)
            {
                case 1:
                    Instantiate(shopMain, new Vector3(cameraEye.position.x, 0, cameraEye.position.z), Quaternion.Euler(new Vector3(0, cameraEye.eulerAngles.y, 0)));
                    break;
                case 2:
                    Instantiate(inventoryMain, new Vector3(cameraEye.position.x, 0, cameraEye.position.z), Quaternion.Euler(new Vector3(0, cameraEye.eulerAngles.y, 0)));
                    break;
                case 3:
                    Instantiate(systemSetting, new Vector3(cameraEye.position.x, 0, cameraEye.position.z), Quaternion.Euler(new Vector3(0, cameraEye.eulerAngles.y, 0)));
                    break;
            }
        }
        else
        {
            //close all
            if (isOpenUI == 1)
            {
                ShopController.Instance().Close();
            }
            else if (isOpenUI == 2)
            {
                InventoryController.Instance().Close();
            }
            else if (isOpenUI == 3)
            {
                //close setting
                SettingController.Instance().Close();
            }

            isOpenUI = 0;
        }
    }

    public static void UpdateUserData()
    {
        //using thread to get userData
        GetUserDataThread tws = new GetUserDataThread(EventManager.UpdateUserData);
        Thread t = new Thread(new ThreadStart(tws.GetUserData));
        t.Start();
    }

    public void CloseUI()
    {
        isOpenUI = 0;
    }

    public void SetIsPointerSelect(bool value)
    {
        isPointerSelect = value;
        if (UIPointerState == 1)
        {
            if (isPointerSelect)
            {
                    radioMenu.SelectObj();
            }
            else
            {
                    radioMenu.DeSelectObj();
            }
        }
        else if (UIPointerState == 2)
        {
            if (isPointerSelect)
            {
                EyetrackerUIController.Instance().SelectObj();
            }
            else
            {
                EyetrackerUIController.Instance().DeSelectObj();
            }
        }
    }

    public bool GetIsPointerSelect()
    {
        return isPointerSelect;
    }

    public void SetIsPointerGrab(bool value)
    {
        isPointerGrab = value;
        if (UIPointerState == 1)
        {
            if (isPointerGrab)
            {
                if (radioMenu)
                    radioMenu.GrabObj();
            }
            else
            {
                if (radioMenu)
                    radioMenu.DeGrabObj();
            }
        }
        if (UIPointerState == 2)
        {
            if (isPointerGrab)
            {
                EyetrackerUIController.Instance().GrabObj();
            }
            else
            {
                EyetrackerUIController.Instance().DeGrabObj();
            }
        }
    }

    public bool GetIsPointerGrab()
    {
        return isPointerGrab;
    }

    public void ReSetIsPointerSelect(bool value)
    {
        isPointerSelect = value;
        if (LastUIPointerState == 1)
        {
            if (isPointerSelect)
            {
                radioMenu.SelectObj();
            }
            else
            {
                radioMenu.DeSelectObj();
            }
        }
        else if (LastUIPointerState == 2)
        {
            if (isPointerSelect)
            {
                EyetrackerUIController.Instance().SelectObj();
            }
            else
            {
                EyetrackerUIController.Instance().DeSelectObj();
            }
        }
    }


    public void ReSetIsPointerGrab(bool value)
    {
        isPointerGrab = value;
        if (LastUIPointerState == 1)
        {
            if (isPointerGrab)
            {
                if (radioMenu)
                    radioMenu.GrabObj();
            }
            else
            {
                if (radioMenu)
                    radioMenu.DeGrabObj();
            }
        }
        if (LastUIPointerState == 2)
        {
            if (isPointerGrab)
            {
                EyetrackerUIController.Instance().GrabObj();
            }
            else
            {
                EyetrackerUIController.Instance().DeGrabObj();
            }
        }
    }

    //Auto scale
    public void AutoScale(float box_size){
        Vector3 scale = obj_point.transform.localScale;
        float size = getTargetSizeByRender(obj_point);
        //Vector3 size = model_obj.GetComponent<MeshFilter>().mesh.bounds.size;
        float ratio = box_size / size;

        obj_point.transform.localScale = new Vector3(scale.x * ratio, scale.y * ratio, scale.z * ratio);
    }


    public float getTargetSizeByRender(GameObject target)
    {
        Vector3 vec = Vector3.one;
        Quaternion localQuaternion = target.transform.rotation;
        target.transform.rotation = Quaternion.identity;
        var renders = target.transform.GetComponentsInChildren<Renderer>();
        if (renders.Length > 0)
        {
            Bounds bounds = renders[0].bounds;
            for (int i = 1; i < renders.Length; i++)
            {
                bounds.Encapsulate(renders[i].bounds);
            }

            if (target.transform.GetComponent<Renderer>())
            {
                bounds.Encapsulate(target.transform.GetComponent<Renderer>().bounds);
            }

            vec = bounds.size;
        }
        else
        {
            vec = target.transform.GetComponent<Renderer>().bounds.size;
        }
        target.transform.rotation = localQuaternion;
        return Mathf.Max(vec.x, vec.y, vec.z);
    }


    /*================
     |Controller event|
      ================*/
    private void DoRTriggerTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerTouch = true;
    }

    private void DoRTriggerUnTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerTouch = false;
    }

    private void DoRTriggerPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerPress = true;
        if (RTriggerPressDown != null)
            RTriggerPressDown();
    }

    private void DoRTriggerUnPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerPress = false;
    }

    private void DoRTriggerClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerClick = true;
        if (RTriggerClickDown != null)
            RTriggerClickDown();
    }

    private void DoRTriggerUnClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerClick = false;
        if (RTriggerClickUp != null)
            RTriggerClickUp();
    }

    private void DoRGripTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripTouch = true;
    }

    private void DoRGripUnTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripTouch = false;
    }

    private void DoRGripPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripPress = true;
    }

    private void DoRGripUnPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripPress = false;
    }

    private void DoRGripClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripClick = true;
        if (RGripClickDown != null)
            RGripClickDown();
    }

    private void DoRGripUnClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripClick = false;
    }

    private void DoLGripClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        ChangeUIPointerState();
    }

    /*===========
    |Canvas event|
    ===========*/
    private void ChangeUIPointerState()
    {
        LastUIPointerState = UIPointerState;
        UIPointerState = (UIPointerState + 1) % 3;

        switch (UIPointerState)
        {
            case 0:
                currentPointerCamera = null;
                break;
            case 1:
                currentPointerCamera = ControllerPointerCamera;
                break;
            case 2:
                currentPointerCamera = EyetrackerPointerCamera;
                break;
            case 3:
                currentPointerCamera = KeyboardPointerCamera;
                break;
        }

        CloseAllUI();

        if (UIPointerEvent != null)
            UIPointerEvent(currentPointerCamera);
    }
}
