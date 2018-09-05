using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MainController : MonoBehaviour {
    [Header("Related Objects")]
    public GameObject ShopMain;
    public GameObject InventoryMain;
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

    public static sqlapi getSqlConnection()
    {
        return sqlConnection;
    }

    public static int GetUserID()
    {
        return userID;
    }

    public void OpenShop(Transform spawnpoint)
    {
        if (!isOpenInventory && !isOpenShop)
        {
            isOpenShop = true;
            Instantiate(ShopMain, spawnpoint.position, spawnpoint.rotation);
        }
    }

    public void OpenInventory(Transform spawnpoint)
    {
        if (!isOpenInventory && !isOpenShop)
        {
            isOpenInventory = true;
            Instantiate(InventoryMain, spawnpoint.position, spawnpoint.rotation);
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

    public static void SetIsSelect(bool value)
    {
        isSelect = value;
        //Call Controller UI here
    }

    public static bool GetIsSelect()
    {
        return isSelect;
    }
}
