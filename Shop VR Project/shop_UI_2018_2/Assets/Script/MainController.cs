using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MainController : MonoBehaviour {
    public GameObject ShopMain;
    public GameObject InventoryMain;
    private static int userID;
    private static bool isOpenShop;
    private static bool isOpenInventory;
    private static sqlapi sqlConnection;

	// Use this for initialization
	void Start () {
        isOpenShop = false;
        isOpenInventory = false;
        sqlConnection = new sqlapi();
        userID = 1;

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
}
