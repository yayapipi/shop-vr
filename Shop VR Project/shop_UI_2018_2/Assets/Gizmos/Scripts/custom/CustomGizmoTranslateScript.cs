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
    public Vector3 hitPoint;
    private Plane plane;
    public GameObject m_Cube;
    private bool firstFrame;
    private Vector3 firstHitPoint;
    private Vector3 offset; //offset between target and hitPoint

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

        mainController = MainController.Instance();
        ChangeGizmoCamera();
        plane = new Plane((gizmoCamera.position - translateTarget.transform.position).normalized, translateTarget.transform.position);

        firstFrame = false;
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
        firstFrame = Input.GetMouseButtonDown(0);

        for (int i = 0; i < 3; i++) {
            if (Input.GetMouseButton(0) && detectors[i].pressing) {

                Vector3 direction = (gizmoCamera.position - translateTarget.transform.position);

                switch (i) {
                    // X Axis
                    case 0:
                        {
                            // If the user is pressing the plane, move along Y and Z, else move along X

                            if (detectors[i].pressingPlane)
                            {
                                plane.SetNormalAndPosition(new Vector3(1, 0, 0), translateTarget.transform.position);
                                rayDetect();
                            }
                            else
                            {
                                plane.SetNormalAndPosition(new Vector3(0, direction.y, direction.z).normalized, translateTarget.transform.position);
                                rayDetect();
                                hitPoint = new Vector3(hitPoint.x, firstHitPoint.y, firstHitPoint.z);
                            }
                        }
                        break;

                    // Y Axis
                    case 1:
                        {
                            // If the user is pressing the plane, move along X and Z, else just move along X

                            if (detectors[i].pressingPlane)
                            {
                                plane.SetNormalAndPosition(new Vector3(0, 1, 0), translateTarget.transform.position);
                                rayDetect();
                            }
                            else
                            {
                                plane.SetNormalAndPosition(new Vector3(direction.x, 0, direction.z).normalized, translateTarget.transform.position);
                                rayDetect();
                                hitPoint = new Vector3(firstHitPoint.x, hitPoint.y, firstHitPoint.z);
                            }
                        }
                        break;

                    // Z Axis
                    case 2:
                        {
                            // If the user is pressing the plane, move along X and Y, else just move along Z
                            
                            if (detectors[i].pressingPlane)
                            {
                                plane.SetNormalAndPosition(new Vector3(0, 0, 1), translateTarget.transform.position);
                                rayDetect();
                            }
                            else
                            {
                                plane.SetNormalAndPosition(new Vector3(direction.x, direction.y, 0).normalized, translateTarget.transform.position);
                                rayDetect();
                                hitPoint = new Vector3(firstHitPoint.x, firstHitPoint.y, hitPoint.z);
                            }
                        }
                        break;
                }

                if (firstFrame)
                {
                    offset = translateTarget.transform.position - hitPoint;
                }

                translateTarget.transform.position = hitPoint + offset;
                transform.position = hitPoint + offset;
                break;
            }
        }
    }

    private void rayDetect()
    {
        Ray hitray = new Ray(gizmoCamera.position, gizmoCamera.forward);

        //Initialise the enter variable
        float enter = 0.0f;

        if (plane.Raycast(hitray, out enter))
        {
            //Get the point that is clicked
            hitPoint = hitray.GetPoint(enter);

            //Move your cube GameObject to the point where you clicked
            //m_Cube.transform.position = hitPoint;
        }

        if (firstFrame)
        {
            firstHitPoint = hitPoint;
        }
    }

}
// End of script.
