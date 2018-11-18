using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelMenuController : MonoBehaviour
{
    public bool isKinematic;
    public bool useGravity;
    public bool showPos;
    public bool showRot;
    private MainController mainController;
    public SwitchAnim KinematicSwitch;
    public SwitchAnim GravitySwitch;
    public SwitchAnim PositionAxisSwitch;
    public SwitchAnim RotationAxisSwitch;

    void Start()
    {
        mainController = MainController.Instance("ModelMenuController");
    }

    public void SwitchModelMenu()
    {
        mainController = MainController.Instance("ModelMenuController");

        if (gameObject.activeSelf)
        {
            DisableModelMenu();
        }
        else
        {
            EnableModelMenu();
        }
    }

    private void EnableModelMenu()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(mainController.cameraEye.position.x, mainController.cameraEye.position.y - 1.24f, mainController.cameraEye.position.z);
            transform.rotation = Quaternion.Euler(new Vector3(0, mainController.cameraEye.eulerAngles.y, 0));
        }

        if(mainController.obj_point == null)
        {
            Debug.Log("obj not exist");
        }

        if (mainController.obj_point.GetComponent<Rigidbody>())
        {
            isKinematic = mainController.obj_isKinematic;
            useGravity = mainController.obj_useGravity;
        }
        else
        {
            Debug.Log("obj rigidbody not exist");
        }

        showPos = false;
        showRot = false;
        if (KinematicSwitch.isOn != isKinematic)
            KinematicSwitch.AnimateSwitch();
        if (GravitySwitch.isOn != useGravity)
            GravitySwitch.AnimateSwitch();
        if (PositionAxisSwitch.isOn != showPos)
            PositionAxisSwitch.AnimateSwitch();
        if (RotationAxisSwitch.isOn != showRot)
            RotationAxisSwitch.AnimateSwitch();
    }

    public void DisableModelMenu()
    {
        if(showPos)
            GizmosModule.Instance().DisableTranslateGizmos();
        if (showRot)
            GizmosModule.Instance().DisbleRotationGizmos();
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }


    public void openKinematic(bool jud)
    {
        isKinematic = jud;
        mainController.obj_isKinematic = jud;
    }

    public void openGravity(bool jud)
    {
        useGravity = jud;
        mainController.obj_useGravity = jud;
    }

    public void showPosAxis()
    {
        showPos = !showPos;

        if (showPos)
            GizmosModule.Instance().EnableTranslateGizmos();
        else
            GizmosModule.Instance().DisableTranslateGizmos();
    }

    public void showRotAxis()
    {
        showRot = !showRot;

        if (showRot)
            GizmosModule.Instance().EnableRotationGizmos();
        else
            GizmosModule.Instance().DisbleRotationGizmos();
    }

    public void resPos()
    {
        mainController.obj_point.transform.position = mainController.cameraEye.position + new Vector3(mainController.cameraEye.forward.x, 0, mainController.cameraEye.forward.z) * 5;
    }

    public void resRot()
    {
        mainController.obj_point.transform.rotation = Quaternion.identity;
    }

    public void resSca()
    {
        if (mainController.obj_point.GetComponent<id>())
            mainController.obj_point.transform.localScale = mainController.obj_point.GetComponent<id>().standard_size;
        else
            Debug.Log("id not exist");
    }

    public void SetToPos()
    {
        if (SelectOneObj())
        {
            mainController.condition = 1;
        }
    }

    public void SetToRot()
    {
        if (SelectOneObj())
        {
            mainController.condition = 2;
        }
    }

    public void SetToScale()
    {
        if (SelectOneObj())
        {
            mainController.condition = 3;
        }
    }

    private bool SelectOneObj()
    {
        if(RadioMenuController.getPanelType() <= 10)
        {
            mainController.radioMenu.openpanel(20);
            mainController.radioMenu.SetPanelBack(1);
            return true;
        }

        return false;
    }

}
