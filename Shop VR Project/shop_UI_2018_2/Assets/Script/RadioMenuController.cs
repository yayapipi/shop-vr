using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMenuController : MonoBehaviour {

    public GameObject BasePanel;
    public GameObject ViewRotatePanel;
    public GameObject ModelPanel;
    public GameObject ModelScalePanel;
    public GameObject ModelRotatePanel;

    // Use this for initialization
    void Start () {
        BasePanel.SetActive(true);
        ViewRotatePanel.SetActive(false);
        ModelPanel.SetActive(false);
        ModelScalePanel.SetActive(false);
        ModelRotatePanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openpanel(int jud) {
        switch (jud) {
            case 1:
                //open view rotate panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(true);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                break;
            case 2:
                //open model panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(true);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                break;
            case 3:
                //open model scale panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(true);
                ModelRotatePanel.SetActive(false);
                break;
            case 4:
                //open model rotate panel
                BasePanel.SetActive(false);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(true);
                break;
            default:
                //open base panel
                BasePanel.SetActive(true);
                ViewRotatePanel.SetActive(false);
                ModelPanel.SetActive(false);
                ModelScalePanel.SetActive(false);
                ModelRotatePanel.SetActive(false);
                break;
        }
    }


    
}
