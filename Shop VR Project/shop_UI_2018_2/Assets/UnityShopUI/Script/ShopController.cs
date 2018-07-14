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

public class ShopController : MonoBehaviour
{
    public GameObject itemPanelParent;
    public GameObject itemPanelPrefab;
    private GameObject newItem;
    public GameObject menuPanel;
    public GameObject itemScrollUpButton;
    public GameObject itemScrollDownButton;
    public GameObject itemContent;

    private ScrollRect itemScrollRect;
    private ButtonChecker itemScrollUp;
    private ButtonChecker itemScrollDown;
    private BoxCollider shopSwitch;
    private GameObject mask;

    public int counter;
    public bool isLoadToEnd;
    public bool isLoadingItems;
    private string viewKind;
    private int viewType;
    public int scrollSpeed;
    public int itemsShowOnce;
    public int userID;

    //database
    private sqlapi test;
    private shopitems[] items_data;
    private users user_data;

    // Use this for initialization
    void Start()
    {
        counter = 0;
        isLoadToEnd = false;
        isLoadingItems = false;
        viewKind = "default";
        viewType = 0;
        itemScrollRect = itemContent.transform.parent.GetComponentInParent<ScrollRect>();
        itemScrollUp = itemScrollUpButton.GetComponent<ButtonChecker>();
        itemScrollDown = itemScrollDownButton.GetComponent<ButtonChecker>();
        shopSwitch = this.gameObject.GetComponent<BoxCollider>();
        mask = this.transform.Find("mask").gameObject;

        //database
        test = new sqlapi();
        //show user information
        StartCoroutine(LoadUser(userID));
        StartCoroutine(LoadItems(viewKind, viewType));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(itemScrollRect.verticalNormalizedPosition);

        //load items
        if (itemScrollRect.verticalNormalizedPosition < 0 && !isLoadToEnd && !isLoadingItems)
            StartCoroutine(LoadItems(viewKind, viewType));

        //if (Input.GetKeyDown("space"))

        if (itemScrollUp.buttonPressed)
        {
            itemContent.transform.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        if (itemScrollDown.buttonPressed)
        {
            itemContent.transform.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
        }
    }

    private IEnumerator LoadUser(int id)
    {
        user_data = test.getusers(id);
        yield return null;

        menuPanel.GetComponent<ShopUserController>().set(user_data);  //display texture and other data on UI
    }

    private IEnumerator LoadItems(string kind, int type)
    {
        isLoadingItems = true;

        if(type == 0)
            switch (kind)
            {
                case "default":
                    items_data = test.Rshop_item(1, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
                case "new":
                    items_data = test.Rshop_item(1, "created_at", "desc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
                case "hot":
                    items_data = test.Rshop_item(1, "click_times", "desc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
                case "recommend":
                    items_data = test.Rshop_item(1, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
            }
        else
            switch (kind)
            {
                case "default":
                    items_data = test.Rshop_item(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
                case "new":
                    items_data = test.Rshop_item(1, type, "created_at", "desc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
                case "hot":
                    items_data = test.Rshop_item(1, type, "click_times", "desc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
                case "recommend":
                    items_data = test.Rshop_item(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
                    yield return null;
                    break;
            }

        //items_data = test.Rshop_item(1, "id", "asc", counter * itemsShowOnce, itemsShowOnce);
        //yield return null;

        foreach (shopitems item_data in items_data)
        {
            newItem = Instantiate(itemPanelPrefab, itemPanelParent.transform);
            newItem.transform.localPosition = Vector3.zero;
            newItem.GetComponent<ShopItemController>().set(item_data);  //display texture and other data on UI
            yield return null;
        }

        if (items_data.Length == 0)
        {
            isLoadToEnd = true;
        }

        counter++;

        yield return new WaitForSeconds(1);

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

            foreach (Transform child in itemContent.transform)
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


            foreach (Transform child in itemContent.transform)
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
        Destroy(this.transform.parent.gameObject);
    }
}