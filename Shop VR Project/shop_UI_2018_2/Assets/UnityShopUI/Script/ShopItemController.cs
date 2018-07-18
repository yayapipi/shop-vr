using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour {
    private shopitems data;
    private GameObject newItem;
    public GameObject shopItemInformationPrefab;
    private Transform spawnPoint;
    private Transform shopItemInformationParent;

    // Use this for initialization
    void Start()
    {
        spawnPoint = this.transform.parent.parent.Find("spawn_point");
        shopItemInformationParent = GameObject.Find("shop_main").transform.Find("sub_UI").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void set(shopitems item_data)
    {
        this.data = item_data;

        this.gameObject.transform.Find("name").gameObject.GetComponent<Text>().text = this.data.name;
        this.gameObject.transform.Find("cost").gameObject.GetComponent<Text>().text = ("$ " + this.data.cost);

        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + this.data.pic_url, this.GetComponentInChildren<RawImage>()));

        this.GetComponent<Animation>().Play("item_panel");
    }

    //Download and load texture
    private IEnumerator LoadTextureToObject(string URL, RawImage img)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No Connection Internet");
            yield return null;
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

    public void OpenInformation()
    {
        //newItem = Instantiate(ItemInformationUIPrefab, this.transform.parent.position - 2 * this.transform.parent.forward, this.transform.parent.rotation);
        newItem = Instantiate(shopItemInformationPrefab, spawnPoint.position, spawnPoint.rotation, shopItemInformationParent);
        //newItem.transform.localPosition = Vector3.zero;
        newItem.GetComponentInChildren<ShopItemInformationController>().set(this.data);  //display texture and other data on UI
    }
}
