using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MainController : MonoBehaviour {
    [Header("Related Objects")]
    public GameObject shopMain;
    public GameObject inventoryMain;
    public RadioMenuController radioMenu;
    public Transform cameraEye;
    public VRTK.VRTK_ControllerEvents leftController;
    public VRTK.VRTK_ControllerEvents rightController;
    public Camera ControllerPointerCamera;
    public Camera EyetrackerPointerCamera;
    public Camera KeyboardPointerCamera;
    public static Camera currentPointerCamera;

    public GameObject obj;
    private static int userID;
    private static sqlapi sqlConnection;
    private bool isOpenShop;
    private bool isOpenInventory;

    [Header("Variables")]
    public GameObject obj_point = null;
    private bool isSelect = false;
    public bool isGrab;
    public static bool isViewRotate = false;
    private static MainController _instance = null;

    //Controller state
    [Header("Controller States")]
    public bool RTriggerTouch = false;
    public bool RTriggerPress = false;
    public bool RTriggerClick = false;
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
    public delegate void CanvasUIPointerEventManager(Camera eventCamera, int state);
    public static event CanvasUIPointerEventManager UIPointerEvent;
    public int UIPointerState = 1;

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

    // Use this for initialization
    void Start () {
        isOpenShop = false;
        isOpenInventory = false;
        sqlConnection = new sqlapi();
        userID = 1;

        //Find gameobject
        obj = GameObject.Find("Object");
        if (!obj)
        {
            obj = new GameObject();
        }

        //Update user information
        UpdateUserData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnApplicationQuit()
    {
        sqlConnection.closeSql();
    }

    public static MainController Instance()
    {
        if (_instance == null)
        {
            throw new Exception("UnityMainThreadDispatcher could not find the MainController object.");
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
        if (!isOpenInventory && !isOpenShop)
        {
            isOpenShop = true;
            Instantiate(shopMain, new Vector3(cameraEye.position.x, 0, cameraEye.position.z), Quaternion.Euler(new Vector3(0, cameraEye.eulerAngles.y, 0)));
        }
    }

    public void OpenInventory()
    {
        if (!isOpenInventory && !isOpenShop)
        {
            isOpenInventory = true;
            Instantiate(inventoryMain, new Vector3(cameraEye.position.x, 0, cameraEye.position.z), Quaternion.Euler(new Vector3(0, cameraEye.eulerAngles.y, 0)));
        }
    }

    public static void UpdateUserData()
    {
        //using thread to get userData
        GetUserDataThread tws = new GetUserDataThread(EventManager.UpdateUserData);
        Thread t = new Thread(new ThreadStart(tws.GetUserData));
        t.Start();
    }

    public void CloseShop()
    {
        isOpenShop = false;
    }

    public void CloseInventory()
    {
        isOpenInventory = false; 
    }

    public void SetIsSelect(bool value)
    {
        isSelect = value;
        //Call Controller UI here
        if (isSelect)
        {
            if (radioMenu)
                radioMenu.openpanel(5);
        }
    }

    public bool GetIsSelect()
    {
        return isSelect;
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
        }

        if(UIPointerEvent != null)
            UIPointerEvent(currentPointerCamera, UIPointerState);
    }
}
