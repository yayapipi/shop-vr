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
    private Vector3 firstHitPos;
    private Quaternion firstRotation; //offset between target and hitPoint
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
        if (gizmoCamera)
        {
            firstFrame = Input.GetMouseButtonDown(0);
            targetPos = rotateTarget.transform.position;

            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButton(0) && detectors[i].pressing)
                {

                    switch (i)
                    {
                        // X Axis
                        case 0:
                            axis = rotateTarget.transform.right.normalized;
                            plane.SetNormalAndPosition(axis, targetPos);
                            rayDetect();
                            offset =  Quaternion.AngleAxis(Vector3.SignedAngle(firstHitPos - targetPos, hitPos - targetPos, axis), axis);
                            //Debug.DrawLine(rotateTarget.transform.position, rotateTarget.transform.right * 100, Color.green);
                            //plane.SetNormalAndPosition(new Vector3(1, 0, 0), rotateTarget.transform.position);

                            break;

                        // Y Axis
                        case 1:
                            axis = rotateTarget.transform.up.normalized;
                            plane.SetNormalAndPosition(axis, targetPos);
                            rayDetect();
                            offset = Quaternion.AngleAxis(Vector3.SignedAngle(firstHitPos - targetPos, hitPos - targetPos, axis), axis);
                            //Debug.DrawLine(rotateTarget.transform.position, rotateTarget.transform.up * 100, Color.green);
                            //plane.SetNormalAndPosition(new Vector3(0, 1, 0), rotateTarget.transform.position);
                            break;

                        // Z Axis
                        case 2:
                            axis = rotateTarget.transform.forward.normalized;
                            plane.SetNormalAndPosition(axis, targetPos);
                            rayDetect();
                            offset = Quaternion.AngleAxis(Vector3.SignedAngle(firstHitPos - targetPos, hitPos - targetPos, axis), axis);
                            //Debug.DrawLine(rotateTarget.transform.position, rotateTarget.transform.forward * 100, Color.green);
                            //plane.SetNormalAndPosition(new Vector3(0, 0, 1), rotateTarget.transform.position);
                            break;
                    }

                    //Debug.Log(Vector3.SignedAngle(firstHitPos - targetPos, hitPos - targetPos, axis));

                    Quaternion finalRotation = offset * rotateTarget.transform.rotation;
                    rotateTarget.transform.rotation = finalRotation;
                    transform.rotation = finalRotation;
                    //Debug.Log("ff: " + finalRotation.eulerAngles);
                    break;
                }
            }
        }
    }

    private void rayDetect()
    {
        Ray hitray = new Ray(gizmoCamera.position, gizmoCamera.forward);

        //Initialise the enter variable
        float enter = 0.0f;
        firstHitPos = hitPos;
        if (plane.Raycast(hitray, out enter))
        {
            //Get the Quaternion
            //hitRotation = Quaternion.LookRotation(hitray.GetPoint(enter) - rotateTarget.transform.position);
            //Debug.Log("hit rotation = " + hitRotation.eulerAngles);
            hitPos = hitray.GetPoint(enter);
        }

        if (firstFrame)
        {
            firstHitPos = hitPos;
            firstRotation = rotateTarget.transform.rotation;
            //offset = rotateTarget.transform.rotation * Quaternion.Inverse(hitRotation);
            //Debug.Log("yes");
        }
    }
}
// End of script.
