using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {
    public Text title;

    public void Set(string text)
    {
        title.text = text;
        GetComponent<Animation>().Play("message");
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
