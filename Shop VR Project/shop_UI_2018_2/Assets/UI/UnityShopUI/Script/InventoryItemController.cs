using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public GameObject shopItemInformationPrefab;

    private Text amountText;
    private GameObject Lock;
    private GameObject UnLock;
    public userinventory userInventData;
    private GameObject newObj;
    private bool isOpenCart;
    public bool serverLock;
    public bool clientLock;

    public void Set(userinventory data)
    {
        //set
        userInventData = data;
        transform.Find("name").gameObject.GetComponent<Text>().text = userInventData.name;
        transform.Find("amount").gameObject.GetComponent<Text>().text = (" "+(userInventData.amount));
        Lock = transform.Find("lock").gameObject;
        UnLock = transform.Find("unlock").gameObject;

        //Load picture
        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + userInventData.pic_url, GetComponentInChildren<RawImage>()));
        //Lock
        if (userInventData.locked)
        {
            Lock.SetActive(true);
            serverLock = true;
            clientLock = true;
        }
        else
        {
            serverLock = false;
            clientLock = false;
            UnLock.SetActive(true);
        }
    }

    public void SubmitAmount(int newAmount)
    {
        if (newAmount > 0)
        {
            transform.Find("amount").gameObject.GetComponent<Text>().text = (" " + (newAmount));
            userInventData.amount = newAmount;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OpenInformation()
    {
        Transform spawnPoint = InventoryController.Instance().itemInformationSpawnPoint;
        newObj = Instantiate(shopItemInformationPrefab, spawnPoint.position, spawnPoint.rotation, InventoryController.Instance().GetSubUI());
        newObj.GetComponentInChildren<ShopItemInformationController>().Set(userInventData, clientLock, this);
    }

    public void SetLock()
    {
        if (clientLock)
        {
            clientLock = false;
            Lock.SetActive(false);
            UnLock.SetActive(true);
        }
        else
        {
            clientLock = true;
            Lock.SetActive(true);
            UnLock.SetActive(false);
        }
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

    void OnDestroy()
    {
        /*
        if (serverLock != clientLock)
        {
            Debug.Log("update lock : item_id = " + userInventData.item_id);
            InventoryController.UpdateInventoryLock(userInventData.item_id, clientLock);
        }
         * */
    }
    
}
