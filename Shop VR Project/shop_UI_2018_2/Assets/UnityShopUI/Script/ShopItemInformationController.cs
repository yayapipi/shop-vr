using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private int itemID;
    private int amount;
    private int minAmount;
    private int maxAmount;
    private bool isOpenCart;
    private ShopItemController shopItemController;

    public void Set(shopitems data)
    {
        //initialize
        isOpenCart = false;
        minAmount = 1;
        maxAmount = 100;

        //set
        amount = minAmount;
        ShopController.Instance().Disable();
        itemID = data.item_id;
        informationContent.Find("name").gameObject.GetComponent<Text>().text = data.name;
        informationContent.Find("cost").gameObject.GetComponent<Text>().text = "$ " + data.cost;
        informationContent.Find("description_text").gameObject.GetComponent<Text>().text = data.description;
        UpdateAmount();

        //Get and load pictures
        GetShopItemPics(data.item_id);

        //Load model
        StartCoroutine(LoadModel(data.model_name, "http://140.123.101.103:88/project/public/" + data.model_linkurl));
    }

    public void Set(shopcartitems data, ShopItemController shopItemController)
    {
        //initialize
        isOpenCart = true;
        minAmount = 1;
        maxAmount = 100;

        //set
        this.shopItemController = shopItemController;
        amount = data.amount;
        CartController.Instance().Disable();
        itemID = data.item_id;
        informationContent.Find("name").gameObject.GetComponent<Text>().text = data.name;
        informationContent.Find("cost").gameObject.GetComponent<Text>().text = "$ " + data.cost;
        informationContent.Find("description_text").gameObject.GetComponent<Text>().text = data.description;
        UpdateAmount();

        //Get and load pictures
        GetShopItemPics(data.item_id);

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
        gameObject.SetActive(false);
        ShopController.Buy(itemID, amount, BuyFinished);
    }

    private void BuyFinished()
    { 
        GameObject newObj = Instantiate(messageBuyPrefab, ShopController.Instance().messageSpawnPoint.position, ShopController.Instance().messageSpawnPoint.rotation);
        newObj.GetComponent<MessageController>().Set(EventManager.GetMessage());
        Close();
    }

    public void Cart()
    {
        if(isOpenCart)
            shopItemController.SubmitAmount(amount);
        gameObject.SetActive(false);
        ShopController.Cart(itemID, amount, CartFinished);
    }

    private void CartFinished()
    {
        GameObject newObj = Instantiate(messageCartPrefab, ShopController.Instance().messageSpawnPoint.position, ShopController.Instance().messageSpawnPoint.rotation);
        newObj.GetComponent<MessageController>().Set(amount + " items");
        Close();
    }

    private void GetShopItemPics(int itemID)
    {
        ShopController.GetShopItemPics(itemID, GetShopItemPicsFinished());
    }

    private IEnumerator GetShopItemPicsFinished()
    {
        pics[] shopItemPics = EventManager.GetShopItemPics();
        GameObject newPicture;

        foreach (pics picture in shopItemPics)
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
            yield return null;
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
        {
            CartController.Instance().Enable();
        }
        else
            ShopController.Instance().Enable();

        Destroy(transform.parent.gameObject);
    }
}
