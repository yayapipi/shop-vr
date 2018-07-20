using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour {
    [Header("Related Objects")]
    public Transform itemContent;
    public GameObject itemPanelPrefab;
    public GameObject menuPanel;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;

    private GameObject newItem;
    private ScrollRect itemScrollRect;
    private BoxCollider shopSwitch;
    private GameObject mask;
    private MainController mainController;

    [Header("Variables")]
    public int userID;
    [SerializeField] private string viewKind;
    [SerializeField] private int viewType;
    [SerializeField] private bool isLoadToEnd;
    [SerializeField] private bool isLoadingItems;
    [Range(100, 1000)] public int scrollSpeed;
    private int counter;
    [Range(6, 30)] public int itemsShowOnce;

    //database
    private sqlapi sqlConnection;
    private shopitems[] items_data;
    private users user_data;

    void Start()
    {
        counter = 0;
        isLoadToEnd = false;
        isLoadingItems = false;
        viewKind = "default";
        viewType = 0;
        itemScrollRect = itemContent.parent.GetComponentInParent<ScrollRect>();
        shopSwitch = GetComponent<BoxCollider>();
        mask = transform.Find("mask").gameObject;
        mainController = GameObject.Find("Main Camera").GetComponent<MainController>();

        //database
        sqlConnection = mainController.getSqlConnection();
        //show user information
        StartCoroutine(LoadUser(userID));
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

    private IEnumerator LoadUser(int id)
    {
        user_data = sqlConnection.getusers(id);
        menuPanel.GetComponent<ShopUserController>().set(user_data);  //display texture and other data on UI
        yield return null;
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
            newItem = Instantiate(itemPanelPrefab, itemContent);
            newItem.transform.localPosition = Vector3.zero;
            newItem.GetComponent<ShopItemController>().set(item_data);  //display texture and other data on UI
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

    public void Disable()
    {
        shopSwitch.enabled = true;
        mask.SetActive(true);
    }

    public void Enable()
    {
        shopSwitch.enabled = false;
        mask.SetActive(false);
    }

    public void Close()
    {
        mainController.CloseShop();
        Destroy(transform.parent.gameObject);
    }
}