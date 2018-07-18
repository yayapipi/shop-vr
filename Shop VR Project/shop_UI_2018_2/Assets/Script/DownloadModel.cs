using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadModel : MonoBehaviour {
    public string modelName;
    public string url;
	// Use this for initialization
	   // Use this for initialization
    void Start () 
    {
        //url += modelName;
        WWW www = new WWW(url);
        StartCoroutine(WaitForReq(www));
    }

    IEnumerator WaitForReq(WWW www)
    {
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if(www.error == null){
            GameObject obj = (GameObject)bundle.LoadAsset(modelName);
            Instantiate(obj); // **Change its position and rotation 
        }
        else{
            Debug.Log(www.error);
        }
    }  
    
}
