using UnityEngine;
using System.Collections;

/// <summary>
///     Simple script to handle the functionality of the Rotate Gizmo (i.e. rotate the gizmo
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
public class CustomGizmoRotateScript : MonoBehaviour {

    /// <summary>
    ///     Rotation speed scalar
    /// </summary>
    public float rotationSpeed = 75.0f;

    /// <summary>
    ///     X torus of gizmo
    /// </summary>
    public GameObject xTorus;

    /// <summary>
    ///     Y torus of gizmo
    /// </summary>
    public GameObject yTorus;

    /// <summary>
    ///     Z torus of gizmo
    /// </summary>
    public GameObject zTorus;

    /// <summary>
    ///     Target for rotation
    /// </summary>
    public GameObject rotateTarget;

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
    private Quaternion offset;
    private Vector3 axis;
    private Vector3 targetPos;

    /// <summary>
    ///     On wake up
    /// </summary>
    public void Awake() {

        // Get the click detection scripts
        detectors = new CustomGizmoClickDetection[3];
        detectors[0] = xTorus.GetComponent<CustomGizmoClickDetection>();
        detectors[1] = yTorus.GetComponent<CustomGizmoClickDetection>();
        detectors[2] = zTorus.GetComponent<CustomGizmoClickDetection>();
    }

    public void Start()
    {
        // Set the same position for the target and the gizmo
        transform.position = rotateTarget.transform.position;
        transform.rotation = rotateTarget.transform.rotation;
        mainController = MainController.Instance();
        ChangeGizmoCamera();
        plane = new Plane((gizmoCamera.position - rotateTarget.transform.position).normalized, rotateTarget.transform.position);
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
            targetPos = rotateTarget.transform.position;

            for (int i = 0; i < 3; i++)
            {
                if (detectors[i].pressing)
                {

                    switch (i)
                    {
                        // X Axis
                        case 0:
                            axis = rotateTarget.transform.right.normalized;
                            plane.SetNormalAndPosition(axis, targetPos);
                            rayDetect();
                            offset =  Quaternion.AngleAxis(Vector3.SignedAngle(lastHitPos - targetPos, hitPos - targetPos, axis), axis);
                            break;

                        // Y Axis
                        case 1:
                            axis = rotateTarget.transform.up.normalized;
                            plane.SetNormalAndPosition(axis, targetPos);
                            rayDetect();
                            offset = Quaternion.AngleAxis(Vector3.SignedAngle(lastHitPos - targetPos, hitPos - targetPos, axis), axis);
                            break;

                        // Z Axis
                        case 2:
                            axis = rotateTarget.transform.forward.normalized;
                            plane.SetNormalAndPosition(axis, targetPos);
                            rayDetect();
                            offset = Quaternion.AngleAxis(Vector3.SignedAngle(lastHitPos - targetPos, hitPos - targetPos, axis), axis);
                            break;
                    }
                    Quaternion finalRotation = offset * rotateTarget.transform.rotation;
                    rotateTarget.transform.rotation = finalRotation;
                    break;
                }
            }
        }

        transform.position = rotateTarget.transform.position;
        transform.rotation = rotateTarget.transform.rotation;
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
            //Get the Quaternion
            hitPos = hitray.GetPoint(enter);
        }

        if (firstFrame)
        {
            lastHitPos = hitPos;
        }
    }
}
// End of script.
