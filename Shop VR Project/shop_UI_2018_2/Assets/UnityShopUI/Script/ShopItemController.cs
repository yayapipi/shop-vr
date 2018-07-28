using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour {
    private shopitems data;
    private GameObject newItem;
    public GameObject shopItemInformationPrefab;
    private Transform spawnPoint;
    private Transform sub_UI;

    public void set(shopitems item_data)
    {
        //initialize
        spawnPoint = transform.parent.parent.Find("spawn_point");
        sub_UI = GameObject.Find("shop_main(Clone)").transform.Find("sub_UI").transform;

        //set
        data = item_data;
        transform.Find("name").gameObject.GetComponent<Text>().text = data.name;
        transform.Find("cost").gameObject.GetComponent<Text>().text = ("$ " + data.cost);

        //Load picture
        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + data.pic_url, GetComponentInChildren<RawImage>()));

        GetComponent<Animation>().Play("item_panel");
    }

    public void OpenInformation()
    {
        newItem = Instantiate(shopItemInformationPrefab, spawnPoint.position, spawnPoint.rotation, sub_UI);
        newItem.GetComponentInChildren<ShopItemInformationController>().set(data);  //display texture and other data on UI
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
    }
}
