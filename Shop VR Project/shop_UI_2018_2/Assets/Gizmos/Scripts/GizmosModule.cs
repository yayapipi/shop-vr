using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosModule : MonoBehaviour {
    public GameObject Capsule;
    public CustomGizmoTranslateScript translateGizmos;
    public CustomGizmoRotateScript rotationGizmos;
    private bool isOpenTranslate;
    private bool isOpenRotation;
    private static GizmosModule _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static GizmosModule Instance()
    {
        if (_instance == null)
        {
            throw new Exception(" could not find the Gizmos object.");
        }
        return _instance;
    }

	// Use this for initialization
	void Start ()
    {
		
	}

    public void EnableTranslateGizmos()
    {
        gameObject.SetActive(true);
        translateGizmos.gameObject.SetActive(true);
        translateGizmos.translateTarget = MainController.Instance().obj_point;
    }

    public void EnableRotationGizmos()
    {
        gameObject.SetActive(true);
        rotationGizmos.gameObject.SetActive(true);
        Capsule.SetActive(true);
        rotationGizmos.rotateTarget = MainController.Instance().obj_point;
    }

    public void DisableTranslateGizmos()
    {
        translateGizmos.gameObject.SetActive(false);
        translateGizmos.translateTarget = MainController.Instance().obj_point;
    }

    public void DisbleRotationGizmos()
    {
        rotationGizmos.gameObject.SetActive(false);
        Capsule.SetActive(false);
        rotationGizmos.rotateTarget = MainController.Instance().obj_point;
    }

    public bool isGizmosOpen()
    {
        return (translateGizmos.gameObject.activeSelf || rotationGizmos.gameObject.activeSelf);
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
