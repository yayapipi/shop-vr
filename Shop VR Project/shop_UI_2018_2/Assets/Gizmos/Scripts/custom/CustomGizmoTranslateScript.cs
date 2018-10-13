using UnityEngine;
using System.Collections;

/// <summary>
///     Simple script to handle the functionality of the Translate Gizmo (i.e. move the gizmo
///     and the target object along the axis the user is dragging towards)
/// </summary>
/// 
/// <author>
///     Michael Hillman - thisishillman.co.uk
/// </author>
/// 
/// <version>
///     1.0.0 - 01st January 2016
/// </version>
public class CustomGizmoTranslateScript : MonoBehaviour {

    /// <summary>
    ///     X axis of gizmo
    /// </summary>
    public GameObject xAxisObject;

    /// <summary>
    ///     Y axis of gizmo
    /// </summary>
    public GameObject yAxisObject;

    /// <summary>
    ///     Z axis of gizmo
    /// </summary>
    public GameObject zAxisObject;

    /// <summary>
    ///     Target for translation
    /// </summary>
    public GameObject translateTarget;

    /// <summary>
    ///     Array of detector scripts stored as [x, y, z]
    /// </summary>
    private CustomGizmoClickDetection[] detectors;

    private MainController mainController;
    private Transform gizmoCamera;
    public Vector3 hitPos;
    private Plane plane;
    private bool firstFrame;
    private bool click;
    private Vector3 lastHitPos;
    private Vector3 finalPos;
    private Vector3 axis;
    private Vector3 targetPos;
    private Vector3 direction;

    /// <summary>
    ///     On wake up
    /// </summary>
    public void Awake()
    {
        // Get the click detection scripts
        detectors = new CustomGizmoClickDetection[3];
        detectors[0] = xAxisObject.GetComponent<CustomGizmoClickDetection>();
        detectors[1] = yAxisObject.GetComponent<CustomGizmoClickDetection>();
        detectors[2] = zAxisObject.GetComponent<CustomGizmoClickDetection>();
    }

    public void Start()
    {
        // Set the same position for the target and the gizmo
        transform.position = translateTarget.transform.position;
        transform.rotation = translateTarget.transform.rotation;

        mainController = MainController.Instance();
        ChangeGizmoCamera();
        plane = new Plane((gizmoCamera.position - translateTarget.transform.position).normalized, translateTarget.transform.position);
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
    ///     Once per frame
    /// </summary>
    public void Update()
    {
        GetClick();

        if (gizmoCamera && click)
        {
            targetPos = translateTarget.transform.position;
            direction = (targetPos - gizmoCamera.position);

            for (int i = 0; i < 3; i++)
            {
                if (detectors[i].pressing)
                {
                    switch (i)
                    {
                        // X Axis
                        case 0:
                            {
                                axis = translateTarget.transform.right.normalized;
                                // If the user is pressing the plane, move along Y and Z, else move along X
                                GetFinalPos(detectors[i].pressingPlane);
                            }
                            break;

                        // Y Axis
                        case 1:
                            {
                                axis = translateTarget.transform.up.normalized;
                                // If the user is pressing the plane, move along X and Z, else just move along X
                                GetFinalPos(detectors[i].pressingPlane);
                            }
                            break;

                        // Z Axis
                        case 2:
                            {
                                axis = translateTarget.transform.forward.normalized;
                                // If the user is pressing the plane, move along X and Y, else just move along Z
                                GetFinalPos(detectors[i].pressingPlane);
                            }
                            break;
                    }
                    /*
                    if (firstFrame)
                    {
                        offset = translateTarget.transform.position - hitPos;
                    }*/
                    //offset = hitPos - lastHitPos;
                    //Vector3 finalPos = translateTarget.transform.position + offset;
                    translateTarget.transform.position = finalPos;
                    break;
                }
            }
        }

        transform.position = translateTarget.transform.position;
        transform.rotation = translateTarget.transform.rotation;
    }

    private void GetFinalPos(bool pressing)
    {
        if (pressing)
        {
            plane.SetNormalAndPosition(axis, targetPos);
            rayDetect();
            finalPos = targetPos + hitPos - lastHitPos;
        }
        else
        {
            Vector3 normal = Vector3.Cross(Vector3.Cross(direction, axis), axis).normalized;
            plane.SetNormalAndPosition(normal, targetPos);
            rayDetect();
            finalPos = targetPos + axis * Vector3.Dot(hitPos - lastHitPos, axis);
        }
    }

    private void GetClick()
    {
        switch (mainController.UIPointerState)
        {
            case 0:
                click = false;
                break;
            case 1:
                //controller
                click = mainController.RTriggerClick;
                break;
            case 2:
                //eyetracker
                click = false;
                break;
            case 3:
                //keyboard
                click = Input.GetMouseButton(0);
                break;
        }

        firstFrame = false;
        for (int i = 0; i < 3; i++)
        {
            if (detectors[i].firstFrame)
            {
                firstFrame = true;
            }
        }
    }

    private void rayDetect()
    {
        Ray hitray = new Ray(gizmoCamera.position, gizmoCamera.forward);

        //Initialise the enter variable
        float enter = 0.0f;
        lastHitPos = hitPos;
        if (plane.Raycast(hitray, out enter))
        {
            //Get the point that is clicked
            hitPos = hitray.GetPoint(enter);
        }

        if (firstFrame)
        {
            lastHitPos = hitPos;
        }
    }

}
// End of script.
