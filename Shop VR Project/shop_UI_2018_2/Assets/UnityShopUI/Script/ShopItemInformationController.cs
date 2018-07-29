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
    public GameObject shopItemInformationMaterialPrefab;
    public Transform modelSpawnPoint;
    public Text amountText;
    public GameObject messageBuyPrefab;
    public GameObject messageCartPrefab;

    private int item_id;
    private MainController mainController;
    private ShopController shopController;
    private CartController cartController;
    private GameObject newPicture;
    private GameObject newObj;
    private pics[] pictures;
    private sqlapi sqlConnection;
    private int amount;
    private int minAmount;
    private int maxAmount;
    private bool isOpenCart;

    public void set(shopitems data, MainController controller1, ShopController controller2, CartController controller3)
    {
        //initialize
        mainController = controller1;
        shopController = controller2;
        cartController = controller3;
        isOpenCart = shopController.GetIsOpenCart();
        sqlConnection = mainController.getSqlConnection();
        minAmount = 1;
        maxAmount = 100;
        amount = minAmount;

        //set
        if (isOpenCart)
            cartController.Disable();
        else
            shopController.Disable();
        item_id = data.id;
        informationContent.Find("name").gameObject.GetComponent<Text>().text = data.name;
        informationContent.Find("cost").gameObject.GetComponent<Text>().text = "$ " + data.cost;
        informationContent.Find("description_text").gameObject.GetComponent<Text>().text = data.description;
        UpdateAmount();

        //Get and load pictures
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

    public void Buy()
    {
        shopController.Buy(item_id, amount);
        newObj = Instantiate(messageBuyPrefab, shopController.messageSpawnPoint.position, shopController.messageSpawnPoint.rotation);
        newObj.GetComponent<MessageController>().Set("Thank you");
        Close();
    }

    public void Cart()
    {
        shopController.Cart(item_id, amount);
        newObj = Instantiate(messageCartPrefab, shopController.messageSpawnPoint.position, shopController.messageSpawnPoint.rotation);
        newObj.GetComponent<MessageController>().Set(amount + " items");
        Close();
    }

    private IEnumerator LoadTextures(int id)
    {
        pictures = sqlConnection.Ritem_pic(id, "id", "asc", 0, 100);
        yield return null;
        foreach (pics picture in pictures)
        {
            newPicture = Instantiate(shopItemInformationMaterialPrefab, materialContent);
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
        if (isOpenCart)
            cartController.Enable();
        else
            shopController.Enable();

        Destroy(transform.parent.gameObject);
    }
}
