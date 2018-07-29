using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    public GameObject ShopMain;
    [SerializeField] private int userID;
    private users user_data;
    private bool isOpenShop;
    private sqlapi sqlConnection;
    private GameObject shopObj;

	// Use this for initialization
	void Start () {
        isOpenShop = false;
        sqlConnection = new sqlapi();

        //Update user information
        UpdateUserData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnApplicationQuit()
    {
        sqlConnection.closeSql();
    }

    public sqlapi getSqlConnection()
    {
        return sqlConnection;
    }

    public void OpenShop(Transform spawnpoint)
    {
        if (!isOpenShop)
        {
            isOpenShop = true;
            shopObj = Instantiate(ShopMain, spawnpoint.position, spawnpoint.rotation);
            shopObj.GetComponentInChildren<ShopController>().Set(this);
            shopObj.GetComponentInChildren<ShopController>().UpdateUserData();
        }
    }

    public void UpdateUserData()
    {
        user_data = sqlConnection.getusers(userID);

        //update shop_main
        if (isOpenShop)
            shopObj.GetComponentInChildren<ShopController>().UpdateUserData();
    }

    public users GetUserData()
    {
        return user_data;
    }

    public void CloseShop()
    {
        isOpenShop = false;
    }
}
