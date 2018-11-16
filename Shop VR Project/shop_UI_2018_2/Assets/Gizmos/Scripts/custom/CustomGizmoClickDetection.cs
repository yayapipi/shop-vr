﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
///     Simple script to detect if the attached gameObject is being clicked upon by the user, applied
///     to an individual gizmo handle/element.
/// </summary>
/// 
/// <author>
///     Michael Hillman - thisishillman.co.uk
/// </author>
/// 
/// <version>
///     1.0.0 - 01st January 2016
/// </version>
public class CustomGizmoClickDetection : MonoBehaviour {

    /// <summary>
    ///     Have we already click on one handle?
    /// </summary>
    public static bool ALREADY_CLICKED;

    /// <summary>
    ///     Camera that has been set to the gizmo layer
    /// </summary>
    private Transform gizmoCamera;

    /// <summary>
    ///     Layer upon which gizmos sit
    /// </summary>
    private LayerMask gizmoLayer;

    /// <summary>
    ///     Targets to detect clicks upon, and highlight when a click is detected
    /// </summary>
    public GameObject[] targets;

    /// <summary>
    ///     Highlight material
    /// </summary>
    public Material highlight;

    /// <summary>
    ///     Dictionary of previous materials before a highlight
    /// </summary>
    public Dictionary<MeshRenderer, Material> previousMaterials;

    /// <summary>
    ///     Is the user currently pressing
    /// </summary>
    [HideInInspector]
    public bool pressing = false;
    [HideInInspector]
    public bool pressingLast = false;

    /// <summary>
    ///     Is the user pressing the plane area?
    /// </summary>
    [HideInInspector]
    public bool pressingPlane = false;

    private MainController mainController;
    public bool clickDown;
    public bool clickUp;
    public bool firstFrame;

    /// <summary>
    ///     On wake-up
    /// </summary>
    public void Awake() {
        previousMaterials = new Dictionary<MeshRenderer, Material>();
    }

    private void Start()
    {
        mainController = MainController.Instance("CustomGizmoClickDetection");
        ChangeGizmoCamera();
        gizmoLayer = mainController.gizmoLayer;
    }

    void OnEnable()
    {
        MainController.UIPointerEvent += ChangeEventCamera;
    }

    void OnDisable()
    {
        MainController.UIPointerEvent -= ChangeEventCamera;
    }

    private void ChangeEventCamera(Camera eventCamera)
    {
        ChangeGizmoCamera();
    }

    private void ChangeGizmoCamera()
    {
        switch (mainController.UIPointerState)
        {
            case 0:
                gizmoCamera = null;
                break;
            case 1:
                //controller
                gizmoCamera = MainController.currentPointerCamera.transform;
                break;
            case 2:
                //eyetracker
                gizmoCamera = null;
                break;
            case 3:
                //keyboard
                gizmoCamera = MainController.currentPointerCamera.transform;
                break;
        }
    }

    /// <summary>
    ///     Checks for hits on the target objects, highlighting when found
    /// </summary>
	public void Update () 
    {
        GetClick();

        if (gizmoCamera)
        {
            // If the left mouse button is pressed      
            if (!ALREADY_CLICKED && clickDown)
            {

                // Detect the object(s) the user has clicked
                Ray ray = new Ray(gizmoCamera.transform.position, gizmoCamera.transform.forward);
                //gizmoCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, gizmoLayer);
                bool detected = false;
                pressingPlane = false;

                // Check if object are our targets (skipping the collision if the renderer isn't enabled)
                foreach (RaycastHit hit in hits)
                {
                    if (Array.IndexOf(targets, hit.collider.gameObject) >= 0)
                    {
                        if (!hit.collider.gameObject.GetComponent<Renderer>().enabled) continue;
                        if (hit.collider.gameObject.name.Contains("_plane_")) pressingPlane = true;
                        detected = true;
                        pressing = true;
                    }
                }

                if (detected)
                {
                    // Store the current materials of the targets, then highlight them
                    if (previousMaterials != null) previousMaterials.Clear();

                    foreach (GameObject target in targets)
                    {

                        try
                        {
                            foreach (MeshRenderer renderer in target.GetComponentsInChildren<MeshRenderer>(false))
                            {
                                previousMaterials[renderer] = renderer.sharedMaterial;
                                renderer.material = highlight;
                            }
                        }
                        catch (NullReferenceException exception)
                        {
                            // Perhaps no previous materials could be found?
                        }
                    }

                    ALREADY_CLICKED = true;
                }


            }
            else if (clickUp && previousMaterials.Count > 0)
            {
                // If the left mouse button was released and we haven't un-highlighted yet 

                foreach (MeshRenderer renderer in previousMaterials.Keys)
                {
                    renderer.material = previousMaterials[renderer];
                }
                previousMaterials.Clear();
                pressing = false;
                pressingPlane = false;
                ALREADY_CLICKED = false;
            }
        }

        firstFrame = (pressing && !pressingLast);
        pressingLast = pressing;
    }


    private void GetClick()
    {
        switch (mainController.UIPointerState)
        {
            case 0:
                clickDown = false;
                clickUp = false;
                break;
            case 1:
                //controller
                clickDown = mainController.RTriggerClickDown_bool;
                clickUp = mainController.RTriggerClickUp_bool;
                break;
            case 2:
                //eyetracker
                clickDown = false;
                clickUp = false;
                break;
            case 3:
                //keyboard
                clickDown = Input.GetMouseButtonDown(0);
                clickUp = Input.GetMouseButtonUp(0);
                break;
        }
    }
}
// End of script.
