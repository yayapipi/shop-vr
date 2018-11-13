namespace VRTK.Examples
{
    using UnityEngine;
    using System.Collections;
    using VRTK.Highlighters;

    public class VRTKExample_PointerObjectHighlighterActivator : MonoBehaviour
    {
        public VRTK_DestinationMarker pointer;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;
        public bool logEnterEvent = true;
        public bool logHoverEvent = false;
        public bool logExitEvent = true;
        public bool logSetEvent = true;

        private Transform objParent;
        private bool trackGrab = false;
        private Transform trackPoint;
        private Rigidbody objRigidBody;
        [Tooltip("The maximum amount of velocity magnitude that can be applied to the Interactable Object. Lowering this can prevent physics glitches if Interactable Objects are moving too fast.")]
        public float velocityLimit = float.PositiveInfinity;
        [Tooltip("The maximum amount of angular velocity magnitude that can be applied to the Interactable Object. Lowering this can prevent physics glitches if Interactable Objects are moving too fast.")]
        public float angularVelocityLimit = float.PositiveInfinity;
        [Tooltip("The maximum difference in distance to the tracked position.")]
        public float maxDistanceDelta = 10f;

        private MainController mainController;

        public float align_speed = 10f;
        private float minDistance = 0.001f;
        private Vector3 setToPosition;
        private Quaternion setToRotation;
        private Vector3 setToScale;

        private void Start()
        {
            mainController = MainController.Instance();
            ChangeObjParent();
            trackGrab = false;
        }

        private void Update()
        {
            //DeSelect by mouse
            if(mainController.UIPointerState == 3 && Input.GetMouseButtonDown(1) && mainController.GetIsPointerSelect() && !mainController.GetIsPointerGrab() && mainController.obj_point != null)
            {
                DeSelect();
            }
            //DeGrab by mouse
            if(mainController.UIPointerState == 3 && Input.GetMouseButtonUp(0) && mainController.GetIsPointerGrab())
            {
                DeGrab();
            }
        }

        void FixedUpdate()
        {
            //calculate velocity and apply on object while grabbing
            if (trackGrab)
            {
                Vector3 positionDelta = trackPoint.position - mainController.obj_point.transform.position;
                Quaternion rotationDelta = trackPoint.rotation * Quaternion.Inverse(mainController.obj_point.transform.rotation);

                float angle;
                Vector3 axis;
                rotationDelta.ToAngleAxis(out angle, out axis);

                angle = ((angle > 180) ? angle -= 360 : angle);

                if (angle != 0)
                {
                    Vector3 angularTarget = angle * axis;
                    Vector3 calculatedAngularVelocity = Vector3.MoveTowards(objRigidBody.angularVelocity, angularTarget, maxDistanceDelta);
                    if (angularVelocityLimit == float.PositiveInfinity || calculatedAngularVelocity.sqrMagnitude < angularVelocityLimit)
                    {
                        objRigidBody.angularVelocity = calculatedAngularVelocity;
                    }
                }

                Vector3 velocityTarget = positionDelta / Time.fixedDeltaTime;
                Vector3 calculatedVelocity = Vector3.MoveTowards(objRigidBody.velocity, velocityTarget, maxDistanceDelta);

                if (velocityLimit == float.PositiveInfinity || calculatedVelocity.sqrMagnitude < velocityLimit)
                {
                    objRigidBody.velocity = calculatedVelocity;
                }
            }
        }

        protected virtual void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

            if (pointer != null)
            {
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                pointer.DestinationMarkerSet += DestinationMarkerSet;

                MainController.RTriggerClickUp += RTriggerClickUp;
                MainController.RGripClickDown += RGripClickDown;
            }
            else
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }

            KeyboardClicker.PointerSet += PointerSet;
            KeyboardClicker.PointerEnter += PointerEnter;
            KeyboardClicker.PointerExit += PointerExit;
        }

        protected virtual void OnDisable()
        {
            if (pointer != null)
            {
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;

                MainController.RTriggerClickUp -= RTriggerClickUp;
                MainController.RGripClickDown -= RGripClickDown;
            }

            KeyboardClicker.PointerSet -= PointerSet;
            KeyboardClicker.PointerEnter -= PointerEnter;
            KeyboardClicker.PointerExit -= PointerExit;
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            PointerEnter(e.target.gameObject);

            if (logEnterEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        private void PointerEnter(GameObject obj)
        {
            if (mainController.enablePointerSelect && obj.tag == "Model" && obj != null)
            {
                if (!mainController.GetIsPointerSelect())
                    ToggleHighlight(obj.transform, hoverColor);
                else if (mainController.enablePointerSelectOtherObject && !GameObject.ReferenceEquals(obj, mainController.obj_point) &&
                    !GameObject.ReferenceEquals(obj, mainController.obj_select1) && !GameObject.ReferenceEquals(obj, mainController.obj_select2))
                    ToggleHighlight(obj.transform, hoverColor);
            }
        }

        private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {
            if (logHoverEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER HOVER", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            PointerExit(e.target.gameObject);

            if (logExitEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        private void PointerExit(GameObject obj)
        {
            if (mainController.enablePointerSelect && obj.tag == "Model" && obj != null)
            {
                if (!mainController.GetIsPointerSelect())
                    ToggleHighlight(obj.transform, Color.clear);
                else if (mainController.enablePointerSelectOtherObject && !GameObject.ReferenceEquals(obj, mainController.obj_point) && 
                    !GameObject.ReferenceEquals(obj, mainController.obj_select1) && !GameObject.ReferenceEquals(obj, mainController.obj_select2))
                    ToggleHighlight(obj.transform, Color.clear);
            }
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            PointerSet(e.target.gameObject);

            if (logSetEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        private void PointerSet(GameObject obj)
        {
            if (mainController.obj_select != 0 && mainController.enablePointerSelect && obj.tag == "Model" && obj != null && mainController.enablePointerSelectOtherObject && !GameObject.ReferenceEquals(obj, mainController.obj_point) &&
                !GameObject.ReferenceEquals(obj, mainController.obj_select1) && !GameObject.ReferenceEquals(obj, mainController.obj_select2))
            {
                //Select 2 Obj for alignment
                if (mainController.obj_select == 1)
                {
                    //first object
                    mainController.obj_select1 = obj;
                    ToggleHighlight(obj.transform, Color.red);
                    mainController.descripUI.StartDescription(13);
                    mainController.obj_select = 2;
                }
                else if (mainController.obj_select == 2)
                {
                    //second object
                    mainController.obj_select2 = obj;
                    ToggleHighlight(obj.transform, Color.blue);
                    mainController.obj_select = 3;
                    mainController.enablePointerSelectOtherObject = false;
                    //obj is not null
                    setToPosition = (mainController.obj_select1.transform.position + mainController.obj_select2.transform.position) / 2;
                    StartCoroutine(Align());
                }
                
                //Select 1 obj for model menu
                else if (mainController.obj_select == 10)
                {
                    //first obj
                    mainController.obj_select1 = obj;
                    ToggleHighlight(obj.transform, Color.red);
                    mainController.obj_select = 11;
                    mainController.enablePointerSelectOtherObject = false;
                    //obj is not null
                    switch (mainController.condition)
                    {
                        case 0:
                            Debug.Log("error: mainController.condition == 0");
                            break;
                        case 1:
                            setToPosition = mainController.obj_select1.transform.position;
                            StartCoroutine(SetToPos());
                            break;
                        case 2:
                            setToRotation = mainController.obj_select1.transform.rotation;
                            StartCoroutine(SetToRot());
                            break;
                        case 3:
                            StartCoroutine(SetToSca());
                            break;
                    }
                }

            }

            if (mainController.enablePointerSelect && obj.tag == "Model" && obj != null && !mainController.enablePointerSelectOtherObject && RadioMenuController.getPanelType() != 13)
            {
                switch (mainController.UIPointerState)
                {
                    case 0:
                        break;
                    case 1:
                        if (!mainController.GetIsPointerSelect())
                        {
                            Select(obj);
                        }
                        else if (mainController.obj_point == obj && !mainController.GetIsPointerGrab() && mainController.enablePointerGrab && !GizmosModule.Instance().isGizmosOpen())
                        {
                            TrackGrab(obj);
                        }
                        break;
                    case 2:
                        if (!mainController.GetIsPointerSelect())
                        {
                            Select(obj);
                        }
                        else if (mainController.obj_point == obj && !mainController.GetIsPointerGrab() && mainController.enablePointerGrab)
                        {
                            TrackGrab(obj);
                        }
                        else if (mainController.obj_point == obj && mainController.GetIsPointerGrab())
                        {
                            DeGrab();
                        }
                        break;
                    case 3:
                        if (!mainController.GetIsPointerSelect())
                        {
                            Select(obj);
                        }
                        else if (mainController.obj_point == obj && !mainController.GetIsPointerGrab() && mainController.enablePointerGrab)
                        {
                            TrackGrab(obj);
                        }
                        break;
                }
            }
        }

        private void Select(GameObject obj)
        {
            ToggleHighlight(obj.transform, selectColor);
            mainController.obj_point = obj;
            mainController.SetIsPointerSelect(true);

            if (obj.GetComponent<Rigidbody>())
            {
                objRigidBody = obj.GetComponent<Rigidbody>();
                mainController.obj_useGravity = objRigidBody.useGravity;
                mainController.obj_isKinematic = objRigidBody.isKinematic;

                objRigidBody.useGravity = false;
                objRigidBody.isKinematic = true;
            }
            else
            {
                objRigidBody = null;
            }
        }

        private void DeSelect()
        {
            mainController.obj_point.transform.parent = mainController.obj.transform;
            ToggleHighlight(mainController.obj_point.transform, Color.clear);

            if (objRigidBody)
            {
                objRigidBody.useGravity = mainController.obj_useGravity;
                objRigidBody.isKinematic = mainController.obj_isKinematic;
            }

            mainController.obj_point = null;
            mainController.SetIsPointerSelect(false);
        }

        private void Grab(GameObject obj)
        {
            mainController.obj_point.transform.parent = objParent;
            mainController.SetIsPointerGrab(true);
        }

        private void DeGrab()
        {
            if(trackGrab)
                objRigidBody.isKinematic = true;
            trackGrab = false;
            mainController.obj_point.transform.parent = mainController.obj.transform;
            mainController.SetIsPointerGrab(false);
        }

        private void TrackGrab(GameObject obj)
        {
            if (!objRigidBody)
            {
                Grab(obj);
                return;
            }

            objRigidBody.isKinematic = false;
            trackGrab = true;
            trackPoint = CreateTrackPoint(objParent, obj);
            mainController.trackPoint = trackPoint;

            mainController.obj_point.transform.parent = objParent;
            mainController.SetIsPointerGrab(true);
        }

        private Transform CreateTrackPoint(Transform controllerPoint, GameObject obj)
        {
            Transform returnTrackpoint = null;

            returnTrackpoint = new GameObject("precision_grab").transform;
            returnTrackpoint.SetParent(controllerPoint.transform);
            returnTrackpoint.position = obj.transform.position;
            returnTrackpoint.rotation = obj.transform.rotation;

            return returnTrackpoint;
        }

        private void RTriggerClickUp()
        {
            if (mainController.UIPointerState == 1)
            {
                if (mainController.GetIsPointerGrab() && mainController.obj_point != null)
                {
                    DeGrab();
                }
            }
        }

        //DeSelect depends on RadioMenuController.panel_type and RGripClickDown, but we do not know which script execute first, 
        //so it's necessarily to call RadioMenuController to DeSelect.
        private void RGripClickDown()
        {
            if (mainController.UIPointerState == 1)
            {
                //Deselect
                if (RadioMenuController.getPanelType() == 1 && mainController.GetIsPointerSelect() && !mainController.GetIsPointerGrab() && mainController.obj_point != null)
                {
                    DeSelect();
                }

                //Cancel model Alignment
                if (RadioMenuController.getPanelType() == 13)
                {
                    if (mainController.obj_select == 2 && mainController.obj_select1 != null)
                    {
                        ToggleHighlight(mainController.obj_select1.transform, Color.clear);
                    }

                    if (mainController.obj_select == 3 && mainController.obj_select1 != null && mainController.obj_select2 != null)
                    {
                        ToggleHighlight(mainController.obj_select1.transform, Color.clear);
                        ToggleHighlight(mainController.obj_select2.transform, Color.clear);
                    }

                    mainController.obj_select = 0;
                    mainController.radioMenu.ModelAlignmentCancelFromVRTKHighlighter();
                }
            }
        }

        protected virtual void ToggleHighlight(Transform target, Color color)
        {
            VRTK_BaseHighlighter highligher = (target != null ? target.GetComponentInChildren<VRTK_BaseHighlighter>() : null);
            if (highligher != null)
            {
                highligher.Initialise();
                if (color != Color.clear)
                {
                    highligher.Highlight(color);
                }
                else
                {
                    highligher.Unhighlight();
                }
            }
        }

        private void ChangeObjParent()
        {
            switch (mainController.UIPointerState)
            {
                case 0:
                    objParent = null;
                    break;
                case 1:
                    objParent = mainController.ControllerPointerCamera.transform;
                    break;
                case 2:
                    objParent = mainController.cameraEye;
                    break;
                case 3:
                    objParent = mainController.cameraEye;
                    break;
            }
        }

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }

        private IEnumerator Align()
        {
            while (mainController.obj_point.transform.position != setToPosition)
            {
                if ((mainController.obj_point.transform.position - setToPosition).magnitude > minDistance)
                {
                    //mainController.obj_point.transform.position = Vector3.MoveTowards(mainController.obj_point.transform.position, align_position, mainController.align_speed * Time.deltaTime);
                    mainController.obj_point.transform.position = Vector3.Lerp(mainController.obj_point.transform.position, setToPosition, align_speed * Time.deltaTime);
                }
                else
                {
                    mainController.obj_point.transform.position = setToPosition;
                    break;
                }
                yield return null;
            }
            ToggleHighlight(mainController.obj_select1.transform, Color.clear);
            ToggleHighlight(mainController.obj_select2.transform, Color.clear);
            mainController.obj_select = 0;
            if (RadioMenuController.getPanelType() == 13)
                mainController.radioMenu.ModelAlignmentCancelFromVRTKHighlighter();
        }

        private IEnumerator SetToPos()
        {
            while (mainController.obj_point.transform.position != setToPosition)
            {
                if ((mainController.obj_point.transform.position - setToPosition).magnitude > minDistance)
                {
                    //mainController.obj_point.transform.position = Vector3.MoveTowards(mainController.obj_point.transform.position, align_position, mainController.align_speed * Time.deltaTime);
                    mainController.obj_point.transform.position = Vector3.Lerp(mainController.obj_point.transform.position, setToPosition, align_speed * Time.deltaTime);
                }
                else
                {
                    mainController.obj_point.transform.position = setToPosition;
                    break;
                }
                yield return null;
            }
            ToggleHighlight(mainController.obj_select1.transform, Color.clear);
            mainController.obj_select = 0;
            if (RadioMenuController.getPanelType() == 20)
                mainController.radioMenu.SelectOneObjCancelFromVRTKHighlighter();
        }

        private IEnumerator SetToRot()
        {
            while (mainController.obj_point.transform.rotation != setToRotation)
            {
                if (Quaternion.Dot(mainController.obj_point.transform.rotation, setToRotation) > minDistance)
                {
                    //mainController.obj_point.transform.position = Vector3.MoveTowards(mainController.obj_point.transform.position, align_position, mainController.align_speed * Time.deltaTime);
                    mainController.obj_point.transform.rotation = Quaternion.Lerp(mainController.obj_point.transform.rotation, setToRotation, align_speed * Time.deltaTime);
                }
                else
                {
                    mainController.obj_point.transform.rotation = setToRotation;
                    break;
                }
                yield return null;
            }
            ToggleHighlight(mainController.obj_select1.transform, Color.clear);
            mainController.obj_select = 0;
            if (RadioMenuController.getPanelType() == 20)
                mainController.radioMenu.SelectOneObjCancelFromVRTKHighlighter();
        }

        private IEnumerator SetToSca()
        {
            float targetSize = mainController.getTargetSizeByRender(mainController.obj_select1);
            Vector3 scale = mainController.obj_point.transform.localScale;
            float size = mainController.getTargetSizeByRender(mainController.obj_point);
            float ratio = targetSize / size;
            setToScale = new Vector3(scale.x * ratio, scale.y * ratio, scale.z * ratio);

            while (mainController.obj_point.transform.localScale != setToScale)
            {
                if ((mainController.obj_point.transform.localScale - setToScale).magnitude > minDistance)
                {
                    //mainController.obj_point.transform.position = Vector3.MoveTowards(mainController.obj_point.transform.position, align_position, mainController.align_speed * Time.deltaTime);
                    mainController.obj_point.transform.localScale = Vector3.Lerp(mainController.obj_point.transform.localScale, setToScale, align_speed * Time.deltaTime);
                }
                else
                {
                    mainController.obj_point.transform.localScale = setToScale;
                    break;
                }
                yield return null;
            }
            ToggleHighlight(mainController.obj_select1.transform, Color.clear);
            mainController.obj_select = 0;
            if (RadioMenuController.getPanelType() == 20)
                mainController.radioMenu.SelectOneObjCancelFromVRTKHighlighter();
        }
    }
}