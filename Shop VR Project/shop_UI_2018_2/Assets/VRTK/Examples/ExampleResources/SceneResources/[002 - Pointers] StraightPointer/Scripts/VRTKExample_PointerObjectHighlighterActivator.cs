namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;

    public class VRTKExample_PointerObjectHighlighterActivator : MonoBehaviour
    {
        private Transform objParent;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;
        public bool logEnterEvent = true;
        public bool logHoverEvent = false;
        public bool logExitEvent = true;
        public bool logSetEvent = true;
        private MainController mainController;

        //Shop VR Grab Object Variable
        //public bool isHolding = false;
        //public SteamVR_TrackedController controller;

        //Rotate And Enlarge
        public float rotate_speed = 30f;

        void Start()
        {
            mainController = MainController.Instance();
            ChangeObjParent();
        }

        void Update()
        {
        }

        private void RTriggerClickDown()
        {
            if (mainController.UIPointerState == 1)
            {

            }
        }

        private void RTriggerClickUp()
        {
            if (mainController.UIPointerState == 1)
            {
                //DeGrab
                if (mainController.isGrab && mainController.obj_point != null)
                {
                    mainController.obj_point.transform.parent = mainController.obj.transform;
                    mainController.isGrab = false;
                }
            }
        }

        private void RTriggerPressDown()
        {
            if (mainController.UIPointerState == 1)
            {

            }
        }

        private void RGripClickDown()
        {
            if (mainController.UIPointerState == 1)
            {
                //Deselect
                if (RadioMenuController.getPanelType() == 5 && mainController.GetIsSelect() && !mainController.isGrab && mainController.obj_point != null)
                {
                    mainController.obj_point.transform.parent = mainController.obj.transform;
                    ToggleHighlight(mainController.obj_point.transform, Color.clear);
                    mainController.obj_point = null;
                    mainController.SetIsSelect(false);
                }
            }
        }

        protected virtual void OnEnable()
        {
            MainController.UIPointerEvent += ChangeEventCamera;

            MainController.RTriggerClickDown += RTriggerClickDown;
            MainController.RTriggerClickUp += RTriggerClickUp;
            MainController.RTriggerPressDown += RTriggerPressDown;
            MainController.RGripClickDown += RGripClickDown;

            KeyboardClicker.PointerSet += PointerSet;
            KeyboardClicker.PointerEnter += PointerEnter;
            KeyboardClicker.PointerExit += PointerExit;
        }

        protected virtual void OnDisable()
        {
            MainController.UIPointerEvent -= ChangeEventCamera;

            MainController.RTriggerClickDown -= RTriggerClickDown;
            MainController.RGripClickDown -= RTriggerClickUp;
            MainController.RTriggerPressDown -= RTriggerPressDown;
            MainController.RGripClickDown -= RGripClickDown;

            KeyboardClicker.PointerSet -= PointerSet;
            KeyboardClicker.PointerEnter -= PointerEnter;
            KeyboardClicker.PointerExit -= PointerExit;
        }

        /*
        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            if (!mainController.GetIsSelect() && e.target.gameObject.tag == "Model")
                ToggleHighlight(e.target, hoverColor);

            if (logEnterEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
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
            if (!mainController.GetIsSelect())
                ToggleHighlight(e.target, Color.clear);

            if (logExitEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            if (e.target.gameObject.tag == "Model" && scanActivity)
            {
                if (!mainController.GetIsSelect())
                {
                    //Select
                    ToggleHighlight(e.target, selectColor);
                    mainController.obj_point = e.target.gameObject;
                    mainController.SetIsSelect(true);
                    scanActivity = false;
                }
                else if (mainController.obj_point == e.target.gameObject)
                {
                    //Grab
                    mainController.obj_point.transform.parent = pointer.transform;
                    mainController.isGrab = true;
                    scanActivity = false;
                }
            }

            if (logSetEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }*/

        private void ChangeEventCamera(Camera eventCamera)
        {
            
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

        //The tag is Model already, no need to compare tag
        private void PointerSet(GameObject target)
        {
            switch (mainController.UIPointerState)
            {
                case 0:
                    break;
                case 1:
                    if (!mainController.GetIsSelect())
                    {
                        //Select
                        ToggleHighlight(target.transform, selectColor);
                        mainController.obj_point = target;
                        mainController.SetIsSelect(true);
                    }
                    else if (mainController.obj_point == target)
                    {
                        //Grab
                        mainController.obj_point.transform.parent = objParent;
                        mainController.isGrab = true;
                    }
                    break;
                case 2:
                    if (!mainController.GetIsSelect())
                    {
                        //Select
                        ToggleHighlight(target.transform, selectColor);
                        mainController.obj_point = target;
                        mainController.SetIsSelect(true);
                    }
                    else if (mainController.obj_point == target && !mainController.isGrab)
                    {
                        //Grab
                        mainController.obj_point.transform.parent = objParent;
                        mainController.isGrab = true;
                    }
                    else if (mainController.obj_point == target && mainController.isGrab)
                    {
                        //DeGrab
                        mainController.obj_point.transform.parent = mainController.obj.transform;
                        mainController.isGrab = false;
                    }
                    break;
                case 3:
                    if (!mainController.GetIsSelect())
                    {
                        //Select
                        ToggleHighlight(target.transform, selectColor);
                        mainController.obj_point = target;
                        mainController.SetIsSelect(true);
                    }
                    else if (mainController.obj_point == target && !mainController.isGrab)
                    {
                        //Grab
                        mainController.obj_point.transform.parent = objParent;
                        mainController.isGrab = true;
                    }
                    else if (mainController.obj_point == target && mainController.isGrab)
                    {
                        //DeGrab
                        mainController.obj_point.transform.parent = mainController.obj.transform;
                        mainController.isGrab = false;
                    }
                    break;
            }
        }

        //The tag is Model already, no need to compare tag
        private void PointerEnter(GameObject target)
        {
            if (!mainController.GetIsSelect())
                ToggleHighlight(target.transform, hoverColor);
        }

        //The tag is Model already, no need to compare tag
        private void PointerExit(GameObject target)
        {
            if (!mainController.GetIsSelect())
                ToggleHighlight(target.transform, Color.clear);
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

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}