using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeUIMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Enter()
    {
        Debug.Log("Enter");
    }

    public void Exit()
    {
        Debug.Log("Exit");
    }

    public void Hover()
    {
        Debug.Log("Hover");
    }

    public void Click()
    {
        Debug.Log("Click");
    }
}
