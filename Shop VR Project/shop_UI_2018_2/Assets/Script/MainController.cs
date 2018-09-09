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

    public GameObject obj;
    private static int userID;
    private static sqlapi sqlConnection;
    private bool isOpenShop;
    private bool isOpenInventory;

    [Header("Variables")]
    public GameObject obj_point = null;
    private bool isSelect = false;
    public bool isScale = true;
    public bool isRotate = true;
    public bool isViewRotate = false;
    private static MainController _instance = null;

    //Controller State
    [Header("Controller States")]
    public bool RTriggerTouch = false;
    public bool RTriggerPress = false;
    public bool RTriggerClick = false;
    public bool RGripTouch = false;
    public bool RGripPress = false;
    public bool RGripClick = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void OnEnable()
    {
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, true, DoTriggerTouched);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, false, DoTriggerUnTouched);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, DoTriggerPressed);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, false, DoTriggerUnPressed);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, true, DoTriggerClicked);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, false, DoTriggerUnClicked);

        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, true, DoGripTouched);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, false, DoGripUnTouched);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, true, DoGripPressed);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, false, DoGripUnPressed);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, true, DoGripClicked);
        rightController.SubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, false, DoGripUnClicked);

    }

    void OnDisable()
    {
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, true, DoTriggerTouched);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerTouch, false, DoTriggerUnTouched);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, DoTriggerPressed);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerPress, false, DoTriggerUnPressed);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, true, DoTriggerClicked);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.TriggerClick, false, DoTriggerUnClicked);

        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, true, DoGripTouched);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripTouch, false, DoGripUnTouched);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, true, DoGripPressed);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripPress, false, DoGripUnPressed);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, true, DoGripClicked);
        rightController.UnsubscribeToButtonAliasEvent(VRTK.VRTK_ControllerEvents.ButtonAlias.GripClick, false, DoGripUnClicked);
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
                radioMenu.openpanel(2);
        }
        else
        {
            if (radioMenu)
                radioMenu.openpanel(0);
        }
    }

    public bool GetIsSelect()
    {
        return isSelect;
    }

    /*================
     |Controller event|
      ================*/
    private void DoTriggerTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerTouch = true;
    }

    private void DoTriggerUnTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerTouch = false;
    }

    private void DoTriggerPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerPress = true;
    }

    private void DoTriggerUnPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerPress = false;
    }

    private void DoTriggerClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerClick = true;
    }

    private void DoTriggerUnClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RTriggerClick = false;
    }

    private void DoGripTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripTouch = true;
    }

    private void DoGripUnTouched(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripTouch = false;
    }

    private void DoGripPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripPress = true;
    }

    private void DoGripUnPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripPress = false;
    }

    private void DoGripClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripClick = true;
    }

    private void DoGripUnClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        RGripClick = false;
    }
}
