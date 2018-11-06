namespace VRTK.Examples
{
    using UnityEngine;
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
        private MainController mainController;

        private void Start()
        {
            mainController = MainController.Instance();
            ChangeObjParent();
        }

        private void Update()
        {
            //Degrab by mouse
            if(mainController.UIPointerState == 3 && Input.GetMouseButtonDown(1) && mainController.GetIsPointerSelect() && !mainController.GetIsPointerGrab() && mainController.obj_point != null)
            {
                mainController.obj_point.transform.parent = mainController.obj.transform;
                ToggleHighlight(mainController.obj_point.transform, Color.clear);

                if (mainController.obj_point.GetComponent<Rigidbody>())
                {
                    mainController.obj_point.GetComponent<Rigidbody>().useGravity = mainController.obj_useGravity;
                    mainController.obj_point.GetComponent<Rigidbody>().isKinematic = mainController.obj_isKinematic;
                }

                mainController.obj_point = null;
                mainController.SetIsPointerSelect(false);
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
            if (mainController.enablePointerSelect && obj.tag == "Model")
            {
                if (!mainController.GetIsPointerSelect())
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
            if (mainController.enablePointerSelect && obj.tag == "Model")
            {
                if (!mainController.GetIsPointerSelect())
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
            if (mainController.obj_select != 0 && mainController.enablePointerSelect && obj.tag == "Model")
            {
                if (mainController.obj_select == 1)
                {
                    mainController.obj_select1 = obj;
                    ToggleHighlight(obj.transform, Color.red);
                    mainController.obj_select = -1; //Return Call Sucess
                }
                if (mainController.obj_select == 2)
                {
                    mainController.obj_select2 = obj;
                    ToggleHighlight(obj.transform, Color.blue);
                    if (GameObject.ReferenceEquals(mainController.obj_select1, mainController.obj_select2))
                    {
                        mainController.obj_select = -404; // Return Error Code (Same Object Selected)
                    }else{
                    mainController.obj_select = -2; //Return Call Sucess
                    }
                    
                }
            }
            if (mainController.enablePointerSelect && obj.tag == "Model")
            {
                switch (mainController.UIPointerState)
                {
                    case 0:
                        break;
                    case 1:
                        if (!mainController.GetIsPointerSelect())
                        {
                            //Select
                            ToggleHighlight(obj.transform, selectColor);
                            mainController.obj_point = obj;
                            mainController.SetIsPointerSelect(true);

                            if (obj.GetComponent<Rigidbody>())
                            {
                                mainController.obj_useGravity = obj.GetComponent<Rigidbody>().useGravity;
                                mainController.obj_isKinematic = obj.GetComponent<Rigidbody>().isKinematic;

                                obj.GetComponent<Rigidbody>().useGravity = false;
                                obj.GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                        else if (mainController.obj_point == obj && !mainController.GetIsPointerGrab() && mainController.enablePointerGrab)
                        {
                            //Grab
                            mainController.obj_point.transform.parent = objParent;
                            mainController.SetIsPointerGrab(true);
                        }
                        break;
                    case 2:
                        if (!mainController.GetIsPointerSelect())
                        {
                            //Select
                            ToggleHighlight(obj.transform, selectColor);
                            mainController.obj_point = obj;
                            mainController.SetIsPointerSelect(true);

                            if (obj.GetComponent<Rigidbody>())
                            {
                                mainController.obj_useGravity = obj.GetComponent<Rigidbody>().useGravity;
                                mainController.obj_isKinematic = obj.GetComponent<Rigidbody>().isKinematic;
                                obj.GetComponent<Rigidbody>().useGravity = false;
                                obj.GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                        else if (mainController.obj_point == obj && !mainController.GetIsPointerGrab() && mainController.enablePointerGrab)
                        {
                            //Grab
                            mainController.obj_point.transform.parent = objParent;
                            mainController.SetIsPointerGrab(true);
                        }
                        else if (mainController.obj_point == obj && mainController.GetIsPointerGrab())
                        {
                            //DeGrab
                            mainController.obj_point.transform.parent = mainController.obj.transform;
                            mainController.SetIsPointerGrab(false);
                        }
                        break;
                    case 3:
                        if (!mainController.GetIsPointerSelect())
                        {
                            //Select
                            ToggleHighlight(obj.transform, selectColor);
                            mainController.obj_point = obj;
                            mainController.SetIsPointerSelect(true);

                            if (obj.GetComponent<Rigidbody>())
                            {
                                mainController.obj_useGravity = obj.GetComponent<Rigidbody>().useGravity;
                                mainController.obj_isKinematic = obj.GetComponent<Rigidbody>().isKinematic;

                                obj.GetComponent<Rigidbody>().useGravity = false;
                                obj.GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                        else if (mainController.obj_point == obj && !mainController.GetIsPointerGrab() && mainController.enablePointerGrab)
                        {
                            //Grab
                            mainController.obj_point.transform.parent = objParent;
                            mainController.SetIsPointerGrab(true);
                        }
                        else if (mainController.obj_point == obj && mainController.GetIsPointerGrab())
                        {
                            //DeGrab
                            mainController.obj_point.transform.parent = mainController.obj.transform;
                            mainController.SetIsPointerGrab(false);
                        }
                        break;
                }
            }
        }

        private void RTriggerClickUp()
        {
            if (mainController.UIPointerState == 1)
            {
                //DeGrab
                if (mainController.GetIsPointerGrab() && mainController.obj_point != null)
                {
                    mainController.obj_point.transform.parent = mainController.obj.transform;
                    mainController.SetIsPointerGrab(false);
                }
            }
        }

        private void RGripClickDown()
        {
            if (mainController.UIPointerState == 1)
            {
                //Deselect
                if (RadioMenuController.getPanelType() == 1 && mainController.GetIsPointerSelect() && !mainController.GetIsPointerGrab() && mainController.obj_point != null)
                {
                    mainController.obj_point.transform.parent = mainController.obj.transform;
                    ToggleHighlight(mainController.obj_point.transform, Color.clear);

                    if (mainController.obj_point.GetComponent<Rigidbody>())
                    {
                        mainController.obj_point.GetComponent<Rigidbody>().useGravity = mainController.obj_useGravity;
                        mainController.obj_point.GetComponent<Rigidbody>().isKinematic = mainController.obj_isKinematic;
                    }

                    mainController.obj_point = null;
                    mainController.SetIsPointerSelect(false);
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
    }
}