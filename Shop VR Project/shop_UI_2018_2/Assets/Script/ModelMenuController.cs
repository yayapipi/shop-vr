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
    private MainController mController;
    public SwitchAnim KinematicSwitch;
    public SwitchAnim GravitySwitch;
    public SwitchAnim PositionAxisSwitch;
    public SwitchAnim RotationAxisSwitch;

    private static ModelMenuController _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static ModelMenuController Instance()
    {
        if (_instance == null)
        {
            throw new Exception(" could not find the DrawModule object.");
        }
        return _instance;
    }

    void Start()
    {
        mController = MainController.Instance();
    }

    public void SwitchModelMenu()
    {
        mController = MainController.Instance();

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
        gameObject.SetActive(true);

        if(mController.obj_point == null)
        {
            Debug.Log("obj not exist");
        }

        if (mController.obj_point.GetComponent<Rigidbody>())
        {
            isKinematic = mController.obj_isKinematic;
            useGravity = mController.obj_useGravity;
        }
        else
        {
            Debug.Log("obj rigidbody not exist");
        }

        showPos = false;
        showRot = false;
        Debug.Log(KinematicSwitch.isOn);
        if (KinematicSwitch.isOn != isKinematic)
            KinematicSwitch.AnimateSwitch();
        if (GravitySwitch.isOn != useGravity)
            GravitySwitch.AnimateSwitch();
        if (PositionAxisSwitch.isOn != showPos)
            PositionAxisSwitch.AnimateSwitch();
        if (RotationAxisSwitch.isOn != showRot)
            RotationAxisSwitch.AnimateSwitch();
    }

    private void DisableModelMenu()
    {
        if(showPos)
            GizmosModule.Instance().DisableTranslateGizmos();
        if (showRot)
            GizmosModule.Instance().DisbleRotationGizmos();

        gameObject.SetActive(false);
    }


    public void openKinematic(bool jud)
    {
        isKinematic = jud;
        mController.obj_isKinematic = jud;
    }

    public void openGravity(bool jud)
    {
        useGravity = jud;
        mController.obj_useGravity = jud;
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
        mController.obj_point.transform.position = mController.cameraEye.position + new Vector3(mController.cameraEye.forward.x, 0, mController.cameraEye.forward.z) * 5;
    }

    public void resRot()
    {
        mController.obj_point.transform.rotation = Quaternion.identity;
    }

    public void resSca()
    {
        if (mController.obj_point.GetComponent<id>())
            mController.obj_point.transform.localScale = mController.obj_point.GetComponent<id>().standard_size;
        else
            Debug.Log("id not exist");
    }
}
