using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manage_database : MonoBehaviour {
    private MainController mainController;
	// Use this for initialization
	void Start () 
    {
        mainController = MainController.Instance();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void UpdateStandardSize()
    {
        if (mainController.obj_point)
        {

        }
        else
        {
            Debug.Log("Error: Please select the object first");
        }
    }

    public void CalculateMaximumLength()
    {
        if (mainController.obj_point)
        {

        }
        else
        {
            Debug.Log("Error: Please select the object first");
        }
    }
}
