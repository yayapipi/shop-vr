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
    [Header("Related Objects")]
    public Transform informationContent;
    public Transform materialContent;
    public GameObject ItemInformationMaterialPrefab;
    public Transform modelSpawnPoint;
    public Text amountText;
    private GameObject newPicture;
    private pics[] pictures;
    private sqlapi sqlConnection;
    private ShopController shopControl;
    private int amount;
    private int minAmount;
    private int maxAmount;

    void Start ()
    {
        //ShopControl = this.transform.root.Find("shop_main(Clone)").GetComponent<ShopController>();
        shopControl = GameObject.Find("shop_main(Clone)").GetComponentInChildren<ShopController>();
        amount = 1;
        minAmount = 1;
        maxAmount = 100;
    }

    public void set(shopitems data)
    {
        Start();
        shopControl.Disable();
        informationContent.Find("name").gameObject.GetComponent<Text>().text = data.name;
        informationContent.Find("cost").gameObject.GetComponent<Text>().text = "$ " + data.cost;
        informationContent.Find("description_text").gameObject.GetComponent<Text>().text = data.description;
        UpdateAmount();

        //get and load pictures
        sqlConnection = GameObject.Find("Main Camera").GetComponent<MainController>().getSqlConnection();
        StartCoroutine(LoadTextures(data.id));

        //Load model
        StartCoroutine(LoadModel(data.model_name, "http://140.123.101.103:88/project/public/" + data.model_linkurl));
    }

    public void IncreaseAmount()
    {
        if(amount < maxAmount)
        {
            amount += 1;
            UpdateAmount();
        }
    }

    public void DecreaseAmount()
    {
        if(amount > minAmount)
        {
            amount -= 1;
            UpdateAmount();
        }
    }

    private void UpdateAmount()
    {
        amountText.text = "amount: " + amount;
    }

    private IEnumerator LoadTextures(int id)
    {
        pictures = sqlConnection.Ritem_pic(id, "id", "asc", 0, 100);
        yield return null;
        foreach (pics picture in pictures)
        {
            newPicture = Instantiate(ItemInformationMaterialPrefab, materialContent);
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
            Instantiate(obj, modelSpawnPoint.position, modelSpawnPoint.rotation, modelSpawnPoint);
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
        Destroy(transform.parent.gameObject);
    }
}
