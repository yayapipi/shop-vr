using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyetrackerUIController : MonoBehaviour {
    private MainController mainController;
    public GameObject panel_0;
    public GameObject panel_1;

    private Image[] panel_Images;

    // Use this for initialization
    void Start()
    {
        panel_Images = new Image[3];

        for (int i = 0; i < 4; i++)
        {
            panel_Images[i] = panel_0.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }

        for (int i = 4; i < 10; i++)
        {
            panel_Images[i] = panel_1.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ViewRotateLeft()
    {
        if (MainController.Instance().UIPointerState == 2)
        {
            mainController.movement.ViewRotateLeft(0.5f);
        }
    }

    public void ViewRotateRight()
    {
        if (MainController.Instance().UIPointerState == 2)
        {
            mainController.movement.ViewRotateRight(0.5f);
        }
    }

    public void Highlight(int button_index)
    {
        if (button_index < 10)
        {
            panel_Images[button_index].color = new Color(0.5f, 0.5f, 0.5f, panel_Images[button_index].color.a);
        }
    }

    public void DeHighlight(int button_index)
    {
        if (button_index < 10)
        {
            panel_Images[button_index].color = new Color(1f, 1f, 1f, panel_Images[button_index].color.a);
        }
    }

    public void Deselect()
    {

    }

    public void Openshop()
    {
        mainController.OpenShop();
    }

    public void OpenInventory()
    {
        mainController.OpenInventory();
    }

    public void OpenSetting()
    {
        mainController.OpenSetting();
    }
}
