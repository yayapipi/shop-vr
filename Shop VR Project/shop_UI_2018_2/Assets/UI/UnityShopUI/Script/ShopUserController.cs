using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUserController : MonoBehaviour {

    void OnEnable()
    {
        EventManager.updateUserData += UpdateUserData;
    }

    void OnDisable()
    {
        EventManager.updateUserData -= UpdateUserData;
    }

    public void Start()
    {
        UpdateUserData();
    }

    public void UpdateUserData()
    {
        users data = EventManager.GetUserData();

        //if(data.pic_linkurl != null)
        //StartCoroutine(LoadTextureToObject("http://140.123.101.103:88/project/public/" + data.pic_linkurl, this.gameObject.transform.Find("mask").gameObject.GetComponentInChildren<RawImage>()));

        this.gameObject.transform.Find("name").gameObject.GetComponent<Text>().text = data.name;
        this.gameObject.transform.Find("money").gameObject.GetComponent<Text>().text = "$ " + data.money;
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
}
