using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBox : MonoBehaviour {
    public bool scale_box = false;
    public float sensitive_value = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (scale_box)
        {
            transform.localScale += new Vector3(sensitive_value, sensitive_value, sensitive_value) * Time.deltaTime;
        }
	}

    public void EnableBox()
    {
        transform.gameObject.SetActive(true);
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        scale_box = true;
        Vector3 position = new Vector3(MainController.Instance().obj_point.transform.position.x, MainController.Instance().obj_point.transform.position.y / 2, MainController.Instance().obj_point.transform.position.z);
        transform.position = position;
    }

}
