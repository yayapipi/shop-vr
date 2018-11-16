using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBoxChild : MonoBehaviour {
    ScaleBox scalebox;
	// Use this for initialization
	void Start () {
        scalebox = GetComponentInParent<ScaleBox>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != MainController.Instance("ScaleBoxChild").obj_point)
        {
            Debug.Log("Collider:" + other.name);
            scalebox.scale_box = false;
            float size = MainController.Instance("ScaleBoxChild").getTargetSizeByRender(transform.parent.gameObject);
            MainController.Instance("ScaleBoxChild").AutoScale(size);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
