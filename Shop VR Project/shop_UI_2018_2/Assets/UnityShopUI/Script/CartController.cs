using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartController : MonoBehaviour
{
    [Header("Related Objects")]
    public Transform itemContent;
    public GameObject cartItemPanelPrefab;
    public GameObject CartItemInformationPrefab;
    public ButtonChecker itemScrollUp;
    public ButtonChecker itemScrollDown;

    private GameObject newItem;
    private GameObject mask;
    private MainController mainController;
    private Transform sub_UI;
    private ShopController shopControl;

    [Header("Variables")]
    public int userID;
    [Range(100, 1000)] public int scrollSpeed;

    //database
    private sqlapi sqlConnection;
    //private shopitems[] items_data;
    private users user_data;

    void Update()
    {
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

    public void set(users data)
    {
        //initialize
        shopControl = GameObject.Find("shop_main(Clone)").GetComponentInChildren<ShopController>();
        sub_UI = GameObject.Find("shop_main(Clone)").transform.Find("sub_UI").transform;
        mainController = GameObject.Find("Main Camera").GetComponent<MainController>();
        mask = transform.Find("mask").gameObject;
        sqlConnection = mainController.getSqlConnection();

        //set
        shopControl.Disable();
        user_data = data;
        //StartCoroutine(LoadItems());
        //CalculateTotalCost
    }

    //private IEnumerator LoadItems(string kind, int type)
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
        shopControl.Enable();
        Destroy(transform.parent.gameObject);
    }
}