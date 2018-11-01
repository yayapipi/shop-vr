using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeUIMenuController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        transform.parent.SetParent(MainController.Instance().cameraEye);
        transform.parent.localPosition = new Vector3(0, -0.6f, 8f);
        transform.parent.localRotation = Quaternion.identity;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //transform.parent.position = MainController.Instance().cameraEye.position;
        //transform.parent.rotation = Quaternion.AngleAxis(MainController.Instance().cameraEye.rotation.eulerAngles.y, Vector3.up);
    }

    public void ViewRotateLeft(bool x)
    {
        MainController.isViewRotateLeft = x;
    }

    public void ViewRotateRight(bool x)
    {
        MainController.isViewRotateRight = x;
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
