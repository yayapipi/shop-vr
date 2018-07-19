using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    public GameObject ShopMain;
    private bool isOpenShop;

	// Use this for initialization
	void Start () {
        isOpenShop = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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
