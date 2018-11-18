using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            Debug.Log("obj: " + child.gameObject.name);
            /*
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.AddComponent<MeshCollider>();
            }*/
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        /*
        Vector3 size = transform.GetComponent<Renderer>().bounds.size;
        Vector3 scale = transform.localScale;
        Vector3 extents = transform.GetComponent<Renderer>().bounds.extents;
        Debug.Log("bound : " + size.x + " " + size.y + " " + size.z);
        Debug.Log("scale : " + scale.x + " " + scale.y + " " + scale.z);
        Debug.Log("extents : " + extents.x + " " + extents.y + " " + extents.z);
        */
	}
}
