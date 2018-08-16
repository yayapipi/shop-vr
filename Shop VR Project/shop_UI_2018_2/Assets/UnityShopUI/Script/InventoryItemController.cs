using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public GameObject shopItemInformationPrefab;

    private Text amountText;
    private int amount;
    private userinventory userInventData;
    private shopcartitems shopCartItemsData;
    private GameObject newObj;
    private bool isOpenCart;

    public void Set(userinventory data)
    {
        //set
        userInventData = data;
        transform.Find("name").gameObject.GetComponent<Text>().text = userInventData.name;
        transform.Find("amount").gameObject.GetComponent<Text>().text = (" "+(userInventData.amount));

        //Load picture
        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + userInventData.pic_url, GetComponentInChildren<RawImage>()));
    }

    public void Set(shopcartitems data)
    {
        //set
        shopCartItemsData = data;
        transform.Find("name").gameObject.GetComponent<Text>().text = shopCartItemsData.name;
        transform.Find("cost").gameObject.GetComponent<Text>().text = ("$ " + shopCartItemsData.cost);

        amountText = transform.Find("amount").GetComponent<Text>();
        amount = data.amount;
        amountText.text = amount.ToString();
        Debug.Log(shopCartItemsData.pic_url);
        //Load picture
        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + shopCartItemsData.pic_url, GetComponentInChildren<RawImage>()));
    }

    public void SubmitAmount(int newAmount)
    {
        if (amount != newAmount)
        {
            CartController.Instance().UpdateTotalCost(shopCartItemsData.cost * (newAmount - amount));
            amount = newAmount;
            amountText.text = amount.ToString();
            shopCartItemsData.amount = newAmount;
        }
    }

    public void OpenInformation()
    {
        Transform spawnPoint = InventoryController.Instance().itemInformationSpawnPoint;
        newObj = Instantiate(shopItemInformationPrefab, spawnPoint.position, spawnPoint.rotation, InventoryController.Instance().GetSubUI());
        newObj.GetComponentInChildren<ShopItemInformationController>().Set(userInventData);
    }
    

    //Download and load texture
    private IEnumerator LoadTextureToObject(string URL, RawImage img)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No Connection Internet");
        }
        else
        {
            WWW W = new WWW(URL);
            //Debug.Log("Download image on progress");
            yield return W;

            if (string.IsNullOrEmpty(W.text))
                Debug.Log("Download failed");
            else
            {
                //Debug.Log("Download Succes");
                Texture2D te = W.texture;
                img.texture = te;
            }
        }

        GetComponent<Animation>().Play("item_panel");
    }
}
