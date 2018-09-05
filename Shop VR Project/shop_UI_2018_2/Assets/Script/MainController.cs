using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MainController : MonoBehaviour {
    [Header("Related Objects")]
    public GameObject ShopMain;
    public GameObject InventoryMain;
    public Transform CameraEye;
    public GameObject RadioMenu;
    public static GameObject obj;
    private static int userID;
    private static bool isOpenShop;
    private static bool isOpenInventory;
    private static sqlapi sqlConnection;

    //Controller
    [Header("Controller Variables")]
    public static GameObject obj_point = null;
    [SerializeField] private static bool isSelect = false;
    public static bool isScale = true;
    public static bool isRotate = true;
    public static bool isViewRotate = false;
    private static MainController _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
            Instantiate(ShopMain, new Vector3(CameraEye.position.x, 0, CameraEye.position.z), Quaternion.Euler(new Vector3(0, CameraEye.eulerAngles.y, 0)));
        }
    }

    public void OpenInventory()
    {
        if (!isOpenInventory && !isOpenShop)
        {
            isOpenInventory = true;
            Instantiate(InventoryMain, new Vector3(CameraEye.position.x, 0, CameraEye.position.z), Quaternion.Euler(new Vector3(0, CameraEye.eulerAngles.y, 0)));
        }
    }

    public static void UpdateUserData()
    {
        //using thread to get userData
        GetUserDataThread tws = new GetUserDataThread(EventManager.UpdateUserData);
        Thread t = new Thread(new ThreadStart(tws.GetUserData));
        t.Start();
    }

    public static void CloseShop()
    {
        isOpenShop = false;
    }

    public static void CloseInventory()
    {
        isOpenInventory = false; 
    }

    public void SetIsSelect(bool value)
    {
        isSelect = value;
        //Call Controller UI here
        if (isSelect)
        {
            if (RadioMenu)
                RadioMenu.GetComponent<RadioMenuController>().openpanel(2);
        }
        else
        {
            if (RadioMenu)
                RadioMenu.GetComponent<RadioMenuController>().openpanel(0);
        }
    }

    public static bool GetIsSelect()
    {
        return isSelect;
    }
}
