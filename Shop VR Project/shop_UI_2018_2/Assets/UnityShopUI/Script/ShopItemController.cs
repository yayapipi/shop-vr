using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour {
    public GameObject shopItemInformationPrefab;

    private shopitems item_data;
    private GameObject newObj;

    public void set(shopitems data)
    {
        //set
        item_data = data;
        //Debug.Log("name:" + item_data.name + "cost:" + item_data.cost);
        transform.Find("name").gameObject.GetComponent<Text>().text = item_data.name;
        transform.Find("cost").gameObject.GetComponent<Text>().text = ("$ " + item_data.cost);

        //Load picture
        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + item_data.pic_url, GetComponentInChildren<RawImage>()));
    }

    public void OpenInformation()
    {
        Transform spawnPoint = ShopController.Instance().itemInformationSpawnPoint;
        newObj = Instantiate(shopItemInformationPrefab, spawnPoint.position, spawnPoint.rotation, ShopController.Instance().GetSubUI());
        newObj.GetComponentInChildren<ShopItemInformationController>().set(item_data);
    }

    public void DeleteFromCart()
    {
        ShopController.DeleteFromCart(item_data.id);
        Destroy(transform.gameObject);
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
