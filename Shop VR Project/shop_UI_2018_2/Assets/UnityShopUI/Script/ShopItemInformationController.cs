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
    public GameObject model_obj;

    private int itemID;
    private int amount;
    private int minAmount;
    private int maxAmount;
    private int isOpen; //0 not open 1 open shop 2 open cart 3 open invent
    private ShopItemController shopItemController;

    public void Set(userinventory data)
    {
        //initialize
        isOpen = 3;
        minAmount = 1;
        maxAmount = 100;

        //set
        amount = minAmount;
        InventoryController.Instance().Disable();
        itemID = data.item_id;
        informationContent.Find("name").gameObject.GetComponent<Text>().text = data.name;
        informationContent.Find("cost").gameObject.GetComponent<Text>().text = " " + data.amount;
        informationContent.Find("description_text").gameObject.GetComponent<Text>().text = data.description;
        UpdateAmount();

        //Get and load pictures
        GetInventItemPics(data.item_id);

        //Load model
        StartCoroutine(LoadModel(data.model_name, "http://140.123.101.103:88/project/public/" + data.model_linkurl));

        //Close Back Collision UI
    }

    public void Set(shopitems data)
    {
        //initialize
        isOpen = 1;
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

        //Close Back Collision UI
    }

    public void Set(shopcartitems data, ShopItemController shopItemController)
    {
        //initialize
        isOpen = 2;
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

        //Close Back Collision UI
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

    /*shop ui*/
    public void Buy()
    {
        gameObject.SetActive(false);
        ShopController.Buy(itemID, amount, Close);
    }

    public void Cart()
    {
        if(isOpen == 2)
            shopItemController.SubmitAmount(amount);
        gameObject.SetActive(false);
        ShopController.Cart(itemID, amount, Close);
    }

    /*inventory ui*/
    public void Sell()
    {
        gameObject.SetActive(false);
        InventoryController.Sell(itemID, amount, Close);
    }

    private void GetShopItemPics(int itemID)
    {
        ShopController.GetShopItemPics(itemID, GetShopItemPicsFinished());
    }

    private void GetInventItemPics(int itemID)
    {
        InventoryController.GetShopItemPics(itemID, GetShopItemPicsFinished());
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
        CleanCache();
        WWW www = new WWW(url);
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            GameObject obj = (GameObject)bundle.LoadAsset(name);
            model_obj =  Instantiate(obj, modelSpawnPoint.position, modelSpawnPoint.rotation, modelSpawnPoint);
            model_obj.AddComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>();
            model_obj.AddComponent<Model_Rotate>();
            

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
        if (isOpen == 2)
        {
            CartController.Instance().Enable();
        }
        else if (isOpen == 1)
        {
            ShopController.Instance().Enable();
        }
        else if (isOpen == 3)
        {
            InventoryController.Instance().Enable();
        }
        
        Destroy(transform.parent.gameObject);
    }

    public void Grap()
    {
        //Load model
        Close();
        GameObject mobj = Instantiate(model_obj, modelSpawnPoint.position, modelSpawnPoint.rotation);
        if (mobj)
        {
            mobj.AddComponent<id>().item_id = itemID;
            mobj.AddComponent<MeshCollider>();
            mobj.AddComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>();
            mobj.GetComponent<Model_Rotate>().enabled = false;
            mobj.tag = "Model";
        }
        ShopController.Grab(itemID);

    }

    public static void CleanCache()
    {
        if (Caching.ClearCache())
        {
            Debug.Log("Successfully cleaned the cache.");
        }
        else
        {
            Debug.Log("Cache is being used.");
        }
    } 
}
