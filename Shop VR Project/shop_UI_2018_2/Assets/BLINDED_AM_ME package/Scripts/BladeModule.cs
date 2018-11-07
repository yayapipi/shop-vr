using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeModule : MonoBehaviour {

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
