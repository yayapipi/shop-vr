using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour {
    private static SettingController _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static SettingController Instance()
    {
        if (_instance == null)
        {
            throw new Exception("could not find the SettingController object.");
        }
        return _instance;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Close()
    {
        MainController.Instance("SettingController").CloseUI();
        Destroy(transform.parent.gameObject);
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
