using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Rotate : MonoBehaviour {

    public float rotate_speed = 25f;

	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += new Vector3(0, rotate_speed, 0) * Time.deltaTime;
	}
}
