using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public struct LockItems
{
    public int itemID;
    public bool isLock;
}

public class InventoryController : MonoBehaviour
{
    [Header("Related Objects")]
    public Transform itemContent;
    public GameObject ShopItemPanelPrefab;
    public GameObject cartMainPrefab;
    public GameObject messagePrefab;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;
    public Transform itemInformationSpawnPoint;
    public Transform cartSpawnPoint;
    public Transform messageSpawnPoint;

    private ScrollRect itemScrollRect;
    private GameObject mask;
    private Transform subUI;

    [Header("Variables")]
    //[SerializeField] private string viewKind;
    [SerializeField] private int viewType;
    [SerializeField] private bool isLoadToEnd;
    [SerializeField] private bool isLoadingItems;
    [Range(100, 1000)] public int scrollSpeed;
    [Range(6, 30)] public int itemsShowOnce;

    private static int counter;
    //private static bool isOpenCart;
    private static InventoryController _instance = null;
    private LockItems[] itemArray;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static InventoryController Instance()
    {
        if (_instance == null)
        {
            throw new Exception("could not find the InventoryController object.");
        }
        return _instance;
    }

    void Start()
    {
        //isOpenCart = false;
        counter = 0;
        isLoadToEnd = false;
        isLoadingItems = false;
        //viewKind = "default";
        viewType = 0;
        itemScrollRect = itemContent.parent.GetComponentInParent<ScrollRect>();
        mask = transform.Find("mask").gameObject;
        subUI = transform.parent.Find("sub_UI");

        //show items
        GetInventItems(viewType);
    }

    void Update()
    {
        //load items
        if (itemScrollRect.verticalNormalizedPosition <= 0.01 && !isLoadToEnd && !isLoadingItems)
            GetInventItems(viewType);

        if (itemScrollUp.buttonPressed)
        {
            itemContent.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        if (itemScrollDown.buttonPressed)
        {
            itemContent.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
        }
    }

    /*public void SetViewKind(string kind)
    {
        if (string.Compare(viewKind, kind) != 0)
        {
            viewKind = kind;
            counter = 0;
            isLoadToEnd = false;
            isLoadingItems = false;

            foreach (Transform child in itemContent)
                Destroy(child.gameObject);

            GetShopItems(viewKind, viewType);
        }
    }*/

    public void SetViewType(int type)
    {
        if (viewType != type)
        {
            viewType = type;
            counter = 0;
            isLoadToEnd = false;
            isLoadingItems = false;

            foreach (Transform child in itemContent)
                Destroy(child.gameObject);

            GetInventItems(viewType);
        }
    }

    public Transform GetSubUI()
    {
        return subUI;
    }

    /* Thread sqlapi */
    public static void Sell(int itemID, int amount, Action callbackDelegate)
    {
        Debug.Log("Sell amount = " + amount + " itemID = " + itemID);

        //using thread to buy item
        callbackDelegate += Instance().ShowMessage;
        callbackDelegate += MainController.UpdateUserData;
        InventoryThread tws = new InventoryThread(itemID, amount, callbackDelegate);
        Thread t = new Thread(new ThreadStart(tws.Sell));
        t.Start();
    }

    /*public static void Checkout(Action callbackDelegate)
    {
        Debug.Log("Checkout: ");

        //using thread to checkout item
        callbackDelegate += Instance().ShowMessage;
        callbackDelegate += MainController.UpdateUserData;
        CheckoutThread tws = new CheckoutThread(callbackDelegate);
        Thread t = new Thread(new ThreadStart(tws.Checkout));
        t.Start();
    }
    */

    public void ShowMessage()
    {
        GameObject newObj = Instantiate(messagePrefab, messageSpawnPoint.position, messageSpawnPoint.rotation);
        newObj.GetComponent<MessageController>().Set(EventManager.GetMessage1(), EventManager.GetMessage2());
    }

    private void GetInventItems(int type)
    {
        users userdata = EventManager.GetUserData();
        int userid = userdata.id;
        isLoadingItems = true;

        //using thread to get inventory items
        GetUserInventThread tws;
        tws = new GetUserInventThread(userid, type, counter * itemsShowOnce, itemsShowOnce, GetShopItemsFinished());
        Thread t = new Thread(new ThreadStart(tws.GetUserInvent));
        t.Start();
    }

    private IEnumerator GetShopItemsFinished()
    {
        userinventory[] userInventdata = EventManager.GetUserInventData();
        GameObject newObj;

        foreach (userinventory item in userInventdata)
        {
            newObj = Instantiate(ShopItemPanelPrefab, itemContent);
            newObj.transform.localPosition = Vector3.zero;
            newObj.GetComponent<InventoryItemController>().Set(item);  //display data on UI
            yield return null;
        }

        if (userInventdata.Length == 0)
        {
            isLoadToEnd = true;
        }

        counter++;

        isLoadingItems = false;
    }

    public static void GetShopItemPics(int itemID, IEnumerator callbackEnumerator)
    {
        //using thread to get item pics
        GetShopItemPicsThread tws = new GetShopItemPicsThread(itemID, callbackEnumerator);
        Thread t = new Thread(new ThreadStart(tws.GetShopItemPics));
        t.Start();
    }

    public static void UpdateInventoryLock(LockItems[] itemArray)
    {
        //using thread to UpdateInventoryLock
        UpdateInventoryLockThread tws = new UpdateInventoryLockThread(itemArray);
        Thread t = new Thread(new ThreadStart(tws.UpdateInventoryLock));
        t.Start();
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

    /*public static void CloseCart()
    {
        isOpenCart = false;
    }*/

    public void Close()
    {
        //Update inventory lock
        itemArray = new LockItems[1000];
        int i = 0;

        foreach (Transform child in itemContent)
            if(child.gameObject.activeSelf)
            {
                InventoryItemController child2 = child.GetComponent<InventoryItemController>();
                if (child2.serverLock != child2.clientLock)
                {
                    itemArray[i].itemID = child2.userInventData.item_id;
                    itemArray[i].isLock = child2.clientLock;
                    i++;
                }
            }

        Array.Resize<LockItems>(ref itemArray, i);
        UpdateInventoryLock(itemArray);
        MainController.Instance().CloseInventory();
        Destroy(transform.parent.gameObject);
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
