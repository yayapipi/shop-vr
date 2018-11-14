using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;
using System.Globalization;

public class ShopItemInformationController : MonoBehaviour {
    [Header("Related Objects")]
    public Transform informationContent;
    public Transform materialContent;
    public GameObject shopItemInformationMaterialPrefab;
    public Transform modelSpawnPoint;
    public Text amountText;
    public GameObject model_obj;

    private int itemID;
    private Vector3 standard_scale;
    private int itemAmount;
    private int panelAmount;
    private int minAmount;
    private int maxAmount;
    private int isOpen; //0 not open 1 open shop 2 open cart 3 open invent
    private ShopItemController shopItemController;
    private InventoryItemController inventoryItemController;
    private bool isLock;

    public void Set(shopitems data)
    {
        //initialize
        isOpen = 1;
        minAmount = 1;
        maxAmount = 100;

        //set
        itemAmount = 0;
        panelAmount = minAmount;
        ShopController.Instance().Disable();
        itemID = data.item_id;

        string[] splits = data.standard_scale.Split('x');
        standard_scale = new Vector3(float.Parse(splits[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(splits[1], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(splits[2], CultureInfo.InvariantCulture.NumberFormat));

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
        itemAmount = data.amount;
        panelAmount = data.amount;
        CartController.Instance().Disable();
        itemID = data.item_id;

        string[] splits = data.standard_scale.Split('x');
        standard_scale = new Vector3(float.Parse(splits[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(splits[1], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(splits[2], CultureInfo.InvariantCulture.NumberFormat));

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

    public void Set(userinventory data, bool clientLock, InventoryItemController inventoryItemController)
    {
        //initialize
        isOpen = 3;
        minAmount = 1;
        maxAmount = 100;
        isLock = clientLock;

        //set
        itemAmount = data.amount;
        panelAmount = minAmount;
        this.inventoryItemController = inventoryItemController;
        InventoryController.Instance().Disable();
        itemID = data.item_id;

        string[] splits = data.standard_scale.Split('x');
        standard_scale = new Vector3(float.Parse(splits[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(splits[1], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(splits[2], CultureInfo.InvariantCulture.NumberFormat));

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

    public void IncreaseAmount()
    {
        if(panelAmount < maxAmount)
        {
            panelAmount += 1;
            UpdateAmount();
        }
    }

    public void DecreaseAmount()
    {
        if(panelAmount > minAmount)
        {
            panelAmount -= 1;
            UpdateAmount();
        }
    }

    private void UpdateAmount()
    {
        amountText.text = "amount: " + panelAmount;
    }

    /*shop ui*/
    public void Buy()
    {
        gameObject.SetActive(false);
        ShopController.Buy(itemID, panelAmount, Close);
    }

    public void Cart()
    {
        if(isOpen == 2)
            shopItemController.SubmitAmount(panelAmount);
        gameObject.SetActive(false);
        ShopController.Cart(itemID, panelAmount, Close);
    }

    /*inventory ui*/
    public void Sell()
    {
        gameObject.SetActive(false);
        if (isLock)
        {
            EventManager.SetMessage("Item locked", "Can't not sell");
            InventoryController.Instance().ShowMessage();
            Close();
        }
        else
        {
            if (isOpen == 3 && itemAmount >= panelAmount)
                inventoryItemController.SubmitAmount(itemAmount - panelAmount);
            gameObject.SetActive(false);
            InventoryController.Sell(itemID, panelAmount, Close);
        }
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
            /*
            string[] splits = s_scale.Split('x');
            float scalex = float.Parse(splits[0], CultureInfo.InvariantCulture.NumberFormat);
            float scaley = float.Parse(splits[1], CultureInfo.InvariantCulture.NumberFormat);
            float scalez = float.Parse(splits[2], CultureInfo.InvariantCulture.NumberFormat);
            */
            GameObject obj = (GameObject)bundle.LoadAsset(name);
            model_obj =  Instantiate(obj, modelSpawnPoint.position, modelSpawnPoint.rotation, modelSpawnPoint);

            Debug.Log("scale by mesh");
            Vector3 scale = model_obj.transform.localScale;
            float size =  MainController.Instance().getTargetSizeByRender(model_obj);
            float ratio = 0.5f / size;

            model_obj.transform.localScale = new Vector3(scale.x * ratio, scale.y * ratio, scale.z * ratio);

            /*
            if (model_obj.GetComponent<MeshFilter>() != null)
            {
                Debug.Log("scale by mesh");
                Vector3 scale = model_obj.transform.localScale;
                Vector3 size = model_obj.GetComponent<MeshFilter>().mesh.bounds.size;
                float ratio = 6.5f / Mathf.Max(size.x, size.y, size.z);

                model_obj.transform.localScale = new Vector3(scale.x * ratio, scale.y * ratio, scale.z * ratio);
            }
            else if (model_obj.transform.GetComponent<BoxCollider>() != null)
            {
                Debug.Log("scale by Collider");
                Vector3 scale = model_obj.transform.localScale;
                Vector3 size = model_obj.GetComponent<BoxCollider>().bounds.size;
                float ratio = 6.5f / Mathf.Max(size.x, size.y, size.z);

                model_obj.transform.localScale = new Vector3(scale.x * ratio, scale.y * ratio, scale.z * ratio);
            }
            else
            {
                Debug.Log("Error: no mesh renderer or collider");
            }*/

            model_obj.AddComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>();
            model_obj.AddComponent<Model_Rotate>();
            if (model_obj.GetComponent<Rigidbody>())
            {
                model_obj.GetComponent<Rigidbody>().isKinematic = true;
                model_obj.GetComponent<Rigidbody>().useGravity = false;
            }
            

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
            mobj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            mobj.AddComponent<id>().item_id = itemID;
            mobj.GetComponent<id>().standard_size = standard_scale;

            if(!mobj.GetComponent<MeshCollider>())
                mobj.AddComponent<MeshCollider>();

            mobj.GetComponent<Model_Rotate>().enabled = false;

            mobj.tag = "Model";

            if (!mobj.GetComponent<Rigidbody>())
                mobj.AddComponent<Rigidbody>();

            mobj.GetComponent<Rigidbody>().isKinematic = true;
            mobj.GetComponent<Rigidbody>().useGravity = false;

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
