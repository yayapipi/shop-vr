using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTest : MonoBehaviour {
    public Material mtl;
    public Material mtl2;
    public Texture tx2;
    public Texture tx22;

  //  public Shader shd;
	// Use this for initialization
	void Start () {
        mtl2=Instantiate(mtl, transform.position, transform.rotation);
        tx22 = Instantiate(tx2, transform.position, transform.rotation);
        transform.GetComponent<MeshRenderer>().material = mtl;
        GetComponent<Renderer>().material.SetTexture("_MainTex", tx22);
        GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", tx22);
        GetComponent<Renderer>().material.SetTexture("_BumpMap", tx22);

	}
	
	// Update is called once per frame
	void Update () {
        
    //    transform.GetComponent<Shader>().Equals(shd);
	}
}
