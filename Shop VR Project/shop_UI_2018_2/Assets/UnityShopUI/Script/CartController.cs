using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartController : MonoBehaviour
{
    [Header("Related Objects")]
    public Transform itemContent;
    public GameObject cartItemPanelPrefab;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;

    private MainController mainController;
    private ShopController shopController;
    private GameObject newItem;
    private GameObject mask;
    private Transform sub_UI;

    //for test
    private shopitems data;

    [Header("Variables")]
    public int userID;
    [Range(100, 1000)] public int scrollSpeed;

    //database
    private sqlapi sqlConnection;
    //private shopitems[] items_data;
    private users user_data;

    void Update()
    {
        //RightClick
        if (Input.GetMouseButtonDown(1))
        {
            newItem = Instantiate(cartItemPanelPrefab, itemContent);
            newItem.transform.localPosition = Vector3.zero;
            newItem.GetComponent<ShopItemController>().set(data, mainController, shopController, this);  //display texture and other data on UI
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

    public void Set(MainController controller1, ShopController controller2)
    {
        //for test
        data.id = 2;
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
        mainController = controller1;
        shopController = controller2;
        sub_UI = shopController.GetSubUI();
        mask = transform.Find("mask").gameObject;
        sqlConnection = mainController.getSqlConnection();

        //set
        shopController.Disable();

        //StartCoroutine(LoadItems());
        //CalculateTotalCost
    }

    public void UpdateUserData()
    {
        user_data = mainController.GetUserData();
    }

    public void Checkout()
    {
        shopController.Checkout();
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
        shopController.Enable();
        shopController.CloseCart();
        Destroy(transform.parent.gameObject);
    }
}