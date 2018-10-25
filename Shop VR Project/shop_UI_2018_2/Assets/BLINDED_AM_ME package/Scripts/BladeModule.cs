using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeModule : MonoBehaviour {

    private static BladeModule _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static BladeModule Instance()
    {
        if (_instance == null)
        {
            throw new Exception(" could not find the DrawModule object.");
        }
        return _instance;
    }

	// Use this for initialization
	void Start () {
		
	}

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
