using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    public GameObject ShopMain;
    private bool isOpenShop;
    private sqlapi sqlConnection;

	// Use this for initialization
	void Start () {
        isOpenShop = false;
        sqlConnection = new sqlapi();
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
            Instantiate(ShopMain, spawnpoint.position, spawnpoint.rotation);
        }
    }

    public void CloseShop()
    {
        isOpenShop = false;
    }
}
