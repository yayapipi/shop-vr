using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour {
    private Transform cameraEye;
	// Use this for initialization
	void Start ()
    {
        cameraEye = MainController.Instance("PlayerCollider").cameraEye;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.localPosition = new Vector3(cameraEye.localPosition.x, 0, cameraEye.localPosition.z);
	}
}
