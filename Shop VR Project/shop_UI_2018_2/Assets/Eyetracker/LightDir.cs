using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LightDir : MonoBehaviour
{
	private Transform EyetrackerParent;
	//public GameObject cam;
	//Vector3 dir;
    //private bool counter;
    //private GameObject WatchObj_last;
    //private GameObject WatchObj;

    void Start()
    {
        EyetrackerParent = MainController.Instance("LightDir").EyetrackerPointerCamera.transform.parent;
        //counter = false;
        //WatchObj_last = null;
    }

    // Update is called once per frame
    void Update ()
	{
		//lighter.transform.position = cam.transform.position;
        EyetrackerParent.LookAt(this.gameObject.transform);
        //RayDetect();
    }

    /*
    void RayDetect()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, this.gameObject.transform.position - cam.transform.position, out hit))
        {
            WatchObj = hit.transform.gameObject;

            if (WatchObj.tag == "ClickObj")
            {
                //print(hit.transform.gameObject.name);

                //Click
                if (GameGaze.eyeclick)
                {
                    if(counter)
                        WatchObj.GetComponent<Renderer>().material.color = new Color(1f, 0.82f, 0.45f);
                    else
                        WatchObj.GetComponent<Renderer>().material.color = Color.blue;
                    counter = !counter;
                }
            }
            else if(WatchObj.tag == "AssociateObj")
            {
                //Watch
                if (WatchObj_last != null && WatchObj_last != WatchObj && !GameGaze.eyeclose)
                    WatchObj.GetComponent<GetWatch>().Watch();

                //Click
                if (GameGaze.eyeclick)
                    WatchObj.GetComponent<GetWatch>().Click();
            }

            //Detect Unwatch
            if (WatchObj_last != null && WatchObj_last.tag == "AssociateObj" && WatchObj_last != WatchObj && !GameGaze.eyeclose)
            {
                WatchObj_last.GetComponent<GetWatch>().Unwatch();
            }
                
            WatchObj_last = WatchObj;
        }
        else
        {
            if (WatchObj_last != null && WatchObj_last.tag == "AssociateObj" && !GameGaze.eyeclose)
            {
                WatchObj_last.GetComponent<GetWatch>().Unwatch();
            }
        }
    }*/
}
