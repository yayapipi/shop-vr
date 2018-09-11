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
        private MainController mainController;

        //Shop VR Grab Object Variable
        //public bool isHolding = false;
        //public SteamVR_TrackedController controller;

        //Rotate And Enlarge
        public float rotate_speed = 30f;
        public bool scanActivity = true;

        void Start()
        {
            mainController = MainController.Instance();
        }

        void Update()
        {
            /*
            scanActivity = scanActivity|controller.triggerPressDown;

            //Deselect
            if (controller.triggerPressed && mainController.GetIsSelect() &&  && scanActivity && mainController.obj_point != null)
            {
                mainController.obj_point.transform.parent = mainController.obj.transform;
                ToggleHighlight(mainController.obj_point.transform, Color.clear);
                mainController.obj_point = null;
                mainController.SetIsSelect(false);
                scanActivity = false;
            }
            */
            //DeGrab

           //isHolding = pointer.GetComponent<VRTK_Pointer>().isActiveBtnPress;
           /*if (MainController.GetIsSelect())
           {
               if (MainController.isScale)
               {
                    if (controller.dirY > 0.7)
                    {
                        MainController.obj_point.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
                    }
                    else if (controller.dirY < -0.7)
                    {
                        MainController.obj_point.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
                    }
                }
               if (MainController.isRotate)
               {
                    if (controller.dirX > 0.7)
                    {
                        MainController.obj_point.transform.localEulerAngles -= new Vector3(0, rotate_speed, 0) * Time.deltaTime;
                    }
                    else if (controller.dirX < -0.7)
                    {
                        MainController.obj_point.transform.localEulerAngles += new Vector3(0, rotate_speed, 0) * Time.deltaTime;
                    }
                }
           }*/
        }

        private void RTriggerClickDown()
        {
            /*
            if (KeyboardClicker.rayCastObj != null && KeyboardClicker.rayCastObj.tag == "Model" && scanActivity)
            {
                Debug.Log("in");
                if (!mainController.GetIsSelect())
                {
                    //Select
                    ToggleHighlight(KeyboardClicker.rayCastObj.transform, selectColor);
                    mainController.obj_point = KeyboardClicker.rayCastObj;
                    mainController.SetIsSelect(true);
                    scanActivity = false;
                }
                else if (mainController.obj_point == KeyboardClicker.rayCastObj)
                {
                    //Grab
                    mainController.obj_point.transform.parent = pointer.transform;
                    mainController.isGrab = true;
                    scanActivity = false;
                }
            }
            else if (KeyboardClicker.rayCastObj != null && scanActivity)
            {
                Debug.Log("out");
                Debug.Log(KeyboardClicker.rayCastObj.name);
            }
            else
            {
                Debug.Log("outout");
                Debug.Log("scanActivity = " + scanActivity);
                if (KeyboardClicker.rayCastObj == null)
                {
                    Debug.Log("raycastobj = null");
                }
                else
                    Debug.Log("raycastobj name = " + KeyboardClicker.rayCastObj.name);
            }
             * */
        }

        private void RTriggerClickUp()
        {
            //DeGrab
            if (mainController.isGrab && mainController.obj_point != null)
            {
                mainController.obj_point.transform.parent = mainController.obj.transform;
                mainController.isGrab = false;
            }
        }

        private void RTriggerPressDown()
        {
            Debug.Log("scanActivity = true");
            scanActivity = true;
        }

        private void RGripClickDown()
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

        protected virtual void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

            if (pointer != null)
            {
                /*
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                pointer.DestinationMarkerSet += DestinationMarkerSet;
                 */
            }
            else
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }

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
            if (pointer != null)
            {
                /*
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;
                */
            }

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

        //The tag is Model already, no need to compare tag
        private void PointerSet(GameObject target)
        {
            if (scanActivity)
            {
                if (!mainController.GetIsSelect())
                {
                    //Select
                    ToggleHighlight(target.transform, selectColor);
                    mainController.obj_point = target;
                    mainController.SetIsSelect(true);
                    scanActivity = false;
                }
                else if (mainController.obj_point == target)
                {
                    //Grab
                    mainController.obj_point.transform.parent = pointer.transform;
                    mainController.isGrab = true;
                    scanActivity = false;
                }
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

        private void EyeEnter()
        {

        }

        private void EyeExit()
        {

        }

        private void EyeSet()
        {

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