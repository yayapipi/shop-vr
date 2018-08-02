using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CartController : MonoBehaviour
{
    [Header("Related Objects")]
    public Transform itemContent;
    public GameObject cartItemPanelPrefab;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;

    private GameObject newObj;
    private GameObject mask;
    private static CartController _instance = null;

    [Header("Variables")]
    [Range(100, 1000)]
    public int scrollSpeed;

    //for test
    private shopitems data;
    //database
    private sqlapi sqlConnection;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void Start()
    {
        //for test
        data.id = 3;
        data.name = "bear";
        data.main_type = 2;
        data.sub_type = 2;
        data.description = "熊";
        data.enabled = true;
        data.model_name = "bear";
        data.model_linkurl = "AssetBundles/official.2";
        data.pic_url = "itempics/official2.JPG";
        data.item_id = 1;
        data.cost = 999;
        data.click_times = 0;

        //initialize
        mask = transform.Find("mask").gameObject;
        sqlConnection = MainController.getSqlConnection();
        ShopController.Instance().Disable();

        //StartCoroutine(LoadItems());
        //CalculateTotalCost
    }

    void Update()
    {
        //RightClick
        if (Input.GetMouseButtonDown(1))
        {
            newObj = Instantiate(cartItemPanelPrefab, itemContent);
            newObj.transform.localPosition = Vector3.zero;
            newObj.GetComponent<ShopItemController>().set(data);  //display texture and other data on UI
        }

        if (itemScrollUp.buttonPressed)
        {
            itemContent.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        if (itemScrollDown.buttonPressed)
        {
            itemContent.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
        }
    }

    public static CartController Instance()
    {
        if (_instance == null)
        {
            throw new Exception("UnityMainThreadDispatcher could not find the CartController object.");
        }
        return _instance;
    }

    public void Checkout()
    {
        ShopController.Checkout();
    }

    //private IEnumerator LoadItems()
    //{
    //    items_data = sqlConnection.xxxxxxxx(user_data.id);
    //    yield return null;

    //    foreach (shopitems item_data in items_data)
    //    {
    //        newItem = Instantiate(ShopItemPanelPrefab, itemContent);
    //        newItem.transform.localPosition = Vector3.zero;
    //        newItem.GetComponent<ShopItemController>().set(item_data);  //display texture and other data on UI
    //        yield return null;
    //    }
    //}

    public void Disable()
    {
        mask.SetActive(true);
    }

    public void Enable()
    {
        mask.SetActive(false);
    }

    public void Close()
    {
        ShopController.Instance().Enable();
        ShopController.CloseCart();
        Destroy(transform.parent.gameObject);
    }
}