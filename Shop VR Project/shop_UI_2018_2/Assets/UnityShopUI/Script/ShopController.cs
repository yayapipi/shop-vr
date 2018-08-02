using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
    [Header("Related Objects")]
    public Transform itemContent;
    public GameObject ShopItemPanelPrefab;
    public GameObject cartMainPrefab;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;
    public Transform itemInformationSpawnPoint;
    public Transform cartSpawnPoint;
    public Transform messageSpawnPoint;

    private GameObject newObj;
    private ScrollRect itemScrollRect;
    private GameObject mask;
    private Transform sub_UI;

    [Header("Variables")]
    [SerializeField] private string viewKind;
    [SerializeField] private int viewType;
    [SerializeField] private bool isLoadToEnd;
    [SerializeField] private bool isLoadingItems;
    [Range(100, 1000)] public int scrollSpeed;
    [Range(6, 30)] public int itemsShowOnce;

    private static int counter;
    private static bool isOpenCart;
    private static ShopController _instance = null;

    //database
    private sqlapi sqlConnection;
    private shopitems[] items_data;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static ShopController Instance()
    {
        if (_instance == null)
        {
            throw new Exception("UnityMainThreadDispatcher could not find the ShopController object.");
        }
        return _instance;
    }

    void Start()
    {
        isOpenCart = false;
        counter = 0;
        isLoadToEnd = false;
        isLoadingItems = false;
        viewKind = "default";
        viewType = 0;
        itemScrollRect = itemContent.parent.GetComponentInParent<ScrollRect>();
        mask = transform.Find("mask").gameObject;
        sub_UI = transform.parent.Find("sub_UI");
        sqlConnection = MainController.getSqlConnection();

        //show items
        StartCoroutine(LoadItems(viewKind, viewType));
    }

    void Update()
    {
        //Debug.Log(itemScrollRect.verticalNormalizedPosition);

        //load items
        if (itemScrollRect.verticalNormalizedPosition < 0 && !isLoadToEnd && !isLoadingItems)
            StartCoroutine(LoadItems(viewKind, viewType));

        //if (Input.GetKeyDown("space"))

        if (itemScrollUp.buttonPressed)
        {
            itemContent.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        if (itemScrollDown.buttonPressed)
        {
            itemContent.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
        }
    }

    private IEnumerator LoadItems(string kind, int type)
    {
        isLoadingItems = true;

        if (type == 0)
            switch (kind)
            {
                case "default":
                    items_data = sqlConnection.Rshop_item(1, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    break;
                case "new":
                    items_data = sqlConnection.Rshop_item(1, "created_at", "desc", counter * itemsShowOnce, itemsShowOnce);
                    break;
                case "hot":
                    items_data = sqlConnection.Rshop_item(1, "click_times", "desc", counter * itemsShowOnce, itemsShowOnce);
                    break;
                case "recommend":
                    items_data = sqlConnection.Rshop_item(1, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    break;
            }
        else
            switch (kind)
            {
                case "default":
                    items_data = sqlConnection.Rshop_item(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    break;
                case "new":
                    items_data = sqlConnection.Rshop_item(1, type, "created_at", "desc", counter * itemsShowOnce, itemsShowOnce);
                    break;
                case "hot":
                    items_data = sqlConnection.Rshop_item(1, type, "click_times", "desc", counter * itemsShowOnce, itemsShowOnce);
                    break;
                case "recommend":
                    items_data = sqlConnection.Rshop_item(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    break;
            }

        yield return null;

        foreach (shopitems item_data in items_data)
        {
            newObj = Instantiate(ShopItemPanelPrefab, itemContent);
            newObj.transform.localPosition = Vector3.zero;
            newObj.GetComponent<ShopItemController>().set(item_data);  //display texture and other data on UI
            yield return null;
        }

        if (items_data.Length == 0)
        {
            isLoadToEnd = true;
        }

        counter++;

        //yield return new WaitForSeconds(1);

        isLoadingItems = false;
    }

    public void SetViewKind(string kind)
    {
        if(string.Compare(viewKind, kind) != 0)
        {
            viewKind = kind;
            counter = 0;
            isLoadToEnd = false;
            isLoadingItems = false;

            foreach (Transform child in itemContent)
                Destroy(child.gameObject);

            StartCoroutine(LoadItems(viewKind, viewType));
        }
    }

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

            StartCoroutine(LoadItems(viewKind, viewType));
        }
    }

    public void OpenCart()
    {
        if (!isOpenCart)
        {
            isOpenCart = true;
            Instantiate(cartMainPrefab, cartSpawnPoint.position, cartSpawnPoint.rotation, sub_UI);
        }
    }

    public static bool GetIsOpenCart()
    {
        return isOpenCart;
    }

    public Transform GetSubUI()
    {
        return sub_UI;
    }

    public static void Buy(int item_id, int amount, Action callbackDelegate)
    {
        
        Debug.Log("Buy amount = " + amount + " item_id = " + item_id);

        //using thread to buy item
        callbackDelegate += MainController.UpdateUserData;
        ShopThread tws = new ShopThread(item_id, amount, callbackDelegate);
        Thread t = new Thread(new ThreadStart(tws.Buy));
        t.Start();
    }

    public static void Cart(int item_id, int amount, Action callbackDelegate)
    {
        Debug.Log("Cart: amount = " + amount + " item_id = " + item_id);

        //using thread to cart item
        ShopThread tws = new ShopThread(item_id, amount, callbackDelegate);
        Thread t = new Thread(new ThreadStart(tws.Cart));
        t.Start();
    }

    public static void Checkout()
    {
        Debug.Log("Checkout: ");
        /* call api */
       MainController.UpdateUserData();
    }

    public static void DeleteFromCart(int item_id)
    {
        Debug.Log("Delete item from cart: item_id = " + item_id);
        /* call api */
    }

    public void Disable()
    {
        mask.SetActive(true);
    }

    public void Enable()
    {
        mask.SetActive(false);
    }

    public static void CloseCart()
    {
        isOpenCart = false;
    }

    public void Close()
    {
        MainController.CloseShop();
        Destroy(transform.parent.gameObject);
    }

    void OnDestroy()
    {
        _instance = null;
    }
}