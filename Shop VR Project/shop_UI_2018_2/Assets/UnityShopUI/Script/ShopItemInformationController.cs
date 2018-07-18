using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
using UnityEngine.UI;

public class ShopItemInformationController : MonoBehaviour {
    public GameObject nameObj;
    public GameObject costObj;
    public GameObject descriptionObj;
    public GameObject pictureParent;
    public GameObject ItemInformationPicturePrefab;
    public GameObject modelSpawnPoint;
    private GameObject newPicture;
    private pics[] pictures;
    private sqlapi test;
    private ShopController shopControl;
    //private GameObject newItem;


    // Use this for initialization
    void Start ()
    {
        //ShopControl = this.transform.root.Find("shop_UI_panel").GetComponent<ShopController>();
        shopControl = GameObject.Find("shop_main").GetComponentInChildren<ShopController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void set(shopitems data)
    {
        Start();
        shopControl.Disable();
        nameObj.GetComponent<Text>().text = data.name;
        costObj.GetComponent<Text>().text = ("$ " + data.cost);
        descriptionObj.GetComponent<Text>().text = data.description; 


        //get and load pictures
        test = new sqlapi();
        StartCoroutine(LoadTextures(data.id));

        //Load model
        StartCoroutine(LoadModel(data.model_name, "http://140.123.101.103:88/project/public/" + data.model_linkurl));
    }

    private IEnumerator LoadTextures(int id)
    {
        pictures = test.Ritem_pic(id, "id", "asc", 0, 100);
        yield return null;
        foreach (pics picture in pictures)
        {
            newPicture = Instantiate(ItemInformationPicturePrefab, pictureParent.transform);
            newPicture.transform.localPosition = Vector3.zero;
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("No Connection Internet");
                yield return null;
            }
            else
            {
                WWW W = new WWW("http://140.123.101.103:88/project/public/" + picture.pic_url);
                //Debug.Log("Download image on progress");
                yield return W;

                if (string.IsNullOrEmpty(W.text))
                    Debug.Log("Download failed");
                else
                {
                    //Debug.Log("Download Succes");
                    Texture2D te = W.texture;
                    newPicture.GetComponent<RawImage>().texture = te;
                }
            }
        }
    }

    private IEnumerator LoadModel(string name, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            GameObject obj = (GameObject)bundle.LoadAsset(name);
            Instantiate(obj, modelSpawnPoint.transform.position, modelSpawnPoint.transform.rotation, modelSpawnPoint.transform);
            //newItem = Instantiate(obj, modelSpawnPoint.transform.position, modelSpawnPoint.transform.rotation, modelSpawnPoint.transform);
            //newItem.transform.localPosition = Vector3.zero;
            //newItem.transform.rotation = Quaternion.Euler(modelViewPanel.transform.right);
            //newItem.transform.eulerAngles = new Vector3(gameObj.transform.eulerAngles.x,gameObj.transform.eulerAngles.y + 180,gameObj.transform.eulerAngles.z
            //);
            //newItem.transform.localScale = new Vector3(20, 20, 20); /*modelScale * Vector3.one;*/
            //Debug.Log("SCALE" + 20 * Vector3.one);
        }
        else
        {
            Debug.Log(www.error);
        }
        bundle.Unload(false);
    }

    public void Close()
    {
        shopControl.Enable();
        Destroy(this.transform.parent.gameObject);
    }
}
