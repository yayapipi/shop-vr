using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour {
    public GameObject shopItemInformationPrefab;

    private MainController mainController;
    private ShopController shopController;
    private CartController cartController;
    private shopitems item_data;
    private GameObject newObj;
    private Transform spawnPoint;
    private Transform sub_UI;

    public void set(shopitems data, MainController controller1, ShopController controller2, CartController controller3)
    {
        //initialize
        mainController = controller1;
        shopController = controller2;
        cartController = controller3;
        sub_UI = shopController.GetSubUI();
        spawnPoint = shopController.itemInformationSpawnPoint;

        //set
        item_data = data;
        transform.Find("name").gameObject.GetComponent<Text>().text = item_data.name;
        transform.Find("cost").gameObject.GetComponent<Text>().text = ("$ " + item_data.cost);

        //Load picture
        StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + item_data.pic_url, GetComponentInChildren<RawImage>()));

    }

    public void OpenInformation()
    {
        newObj = Instantiate(shopItemInformationPrefab, spawnPoint.position, spawnPoint.rotation, sub_UI);
        newObj.GetComponentInChildren<ShopItemInformationController>().set(item_data, mainController, shopController, cartController);
    }

    public void DeleteFromCart()
    {
        shopController.DeleteFromCart(item_data.id);
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
