using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    public int t;
	// Use this for initialization
	void Start () {
        t = 0;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.AngleAxis(t, this.transform.up);
        t++;
    }
}
