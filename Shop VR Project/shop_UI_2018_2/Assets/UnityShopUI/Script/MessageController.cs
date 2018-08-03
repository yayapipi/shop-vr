using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {
    public Text title;
    public Text text;

    public void Set(string title, string text)
    {
        this.title.text = title;
        this.text.text = text;
        GetComponent<Animation>().Play("message");
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
