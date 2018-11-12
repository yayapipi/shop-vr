using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CartController : MonoBehaviour
{
    [Header("Related Objects")]
    public Text totalCostText;
    public Transform itemContent;
    public GameObject cartItemPanelPrefab;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;

    private static int totalCost;
    private GameObject mask;
    private static CartController _instance = null;

    [Header("Variables")]
    [Range(100, 1000)]
    public int scrollSpeed;

    //for test
    //private shopcartitems data;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void Start()
    {
        //initialize
        mask = transform.Find("mask").gameObject;
        ShopController.Instance().Disable();
        GetShopCartItems();
        //CalculateTotalCost
    }

    void Update()
    {

    }

    public void ScrollItemByHold(bool dir)
    {
        if (MainController.Instance().UIPointerState == 1 || MainController.Instance().UIPointerState == 3)
            VerticalScroll(itemContent, dir);
    }

    public void ScrollItemByHover(bool dir)
    {
        if (MainController.Instance().UIPointerState == 2)
            VerticalScroll(itemContent, dir);
    }

    private void VerticalScroll(Transform itemContent, bool dir)
    {
        if (dir)
        {
            itemContent.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
        }
        else
        {
            itemContent.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;
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
        ShopController.Checkout(Close);
    }

    public void UpdateTotalCost(int change)
    {
        totalCost += change;
        totalCostText.text = "Total Cost : " + totalCost;
    }

    private void GetShopCartItems()
    {
        ShopController.GetShopCartItems(GetShopCartItemsFinished());
    }

    private IEnumerator GetShopCartItemsFinished()
    {
        shopcartitems[] shopCartItems = EventManager.GetShopCartItems();
        GameObject newObj;
        totalCost = 0;

        foreach (shopcartitems item in shopCartItems)
        {
            totalCost += item.cost * item.amount;
            newObj = Instantiate(cartItemPanelPrefab, itemContent);
            newObj.transform.localPosition = Vector3.zero;
            newObj.GetComponent<ShopItemController>().Set(item);  //display data on UI
            yield return null;
        }

        totalCostText.text = "Total Cost : " + totalCost;
    }

    public void Disable()
    {
        mask.SetActive(true);
        /*
        GetComponent<VRTK.VRTK_UICanvas>().enabled = false;
        foreach (Transform child in itemContent)
            if (child.GetComponent<VRTK.VRTK_UICanvas>() != null)
                child.GetComponent<VRTK.VRTK_UICanvas>().enabled = false;
        */
    }

    public void Enable()
    {
        mask.SetActive(false);
        /*
        GetComponent<VRTK.VRTK_UICanvas>().enabled = true;
        foreach (Transform child in itemContent)
            if (child.GetComponent<VRTK.VRTK_UICanvas>() != null)
                child.GetComponent<VRTK.VRTK_UICanvas>().enabled = true;
        */
    }

    public void Close()
    {
        ShopController.Instance().Enable();
        ShopController.CloseCart();
        Destroy(transform.parent.gameObject);
    }
}