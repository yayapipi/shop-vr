namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;

    public class VRTKExample_PointerObjectHighlighterActivator : MonoBehaviour
    {
        public GameObject control;
        public VRTK_DestinationMarker pointer;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;
        public bool logEnterEvent = true;
        public bool logHoverEvent = false;
        public bool logExitEvent = true;
        public bool logSetEvent = true;

        //Shop VR Grab Object Variable
        public bool isselect = false;
        public bool isHolding = false;
        public GameObject obj_point = null;
        public GameObject controller;

        //Rotate And Enlarge
        public bool isScale = true;
        public bool isRotate = true;
        public float rotate_speed = 30f;
        public bool lastTriggerPressed = false;
        public bool scanActivity = true;

        void Update()
        {
            if (control.GetComponent<SteamVR_TrackedController>().triggerPressed && !lastTriggerPressed)
                scanActivity = true;

            lastTriggerPressed = control.GetComponent<SteamVR_TrackedController>().triggerPressed;

            //Deselect
            if (control.GetComponent<SteamVR_TrackedController>().triggerPressed && obj_point != null && scanActivity)
            {
                GameObject obj = GameObject.Find("Object");
                if (!obj)
                {
                    obj = new GameObject();
                }
                obj_point.transform.parent = obj.transform;
                ToggleHighlight(obj_point.transform, Color.clear);
                obj_point = null;
                isselect = false;
                scanActivity = false;
            }

           isHolding = pointer.GetComponent<VRTK_Pointer>().isActiveBtnPress;
           if (isselect)
           {
               if (isScale)
               {
                   if (controller.GetComponent<SteamVR_TrackedController>().padPressed)
                   {
                       if (controller.GetComponent<SteamVR_TrackedController>().dirY > 0.7)
                       {
                           obj_point.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
                       }
                       else if (controller.GetComponent<SteamVR_TrackedController>().dirY < -0.7)
                       {
                           obj_point.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
                       }
                   }
               }
               if (isRotate)
               {
                   if (controller.GetComponent<SteamVR_TrackedController>().padPressed)
                   {
                       if (controller.GetComponent<SteamVR_TrackedController>().dirX > 0.7)
                       {
                           obj_point.transform.localEulerAngles -= new Vector3(0, rotate_speed, 0) * Time.deltaTime;
                       }
                       else if (controller.GetComponent<SteamVR_TrackedController>().dirX < -0.7)
                       {
                           obj_point.transform.localEulerAngles += new Vector3(0, rotate_speed, 0) * Time.deltaTime;
                       }
                   }
               }
           }
           else
           {
               if (obj_point)
               {
                   GameObject obj = GameObject.Find("Object");
                   if (!obj)
                   {
                       obj = new GameObject();
                   }
                   obj_point.transform.parent = obj.transform;
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
            }
            else
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }
        }

        protected virtual void OnDisable()
        {
            if (pointer != null)
            {
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;
            }
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            if (!isselect && e.target.gameObject.tag == "Model")
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
            if(!isselect)
                ToggleHighlight(e.target, Color.clear);

            if (logExitEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            if (obj_point == null && e.target.gameObject.tag == "Model" && scanActivity)
            {
                ToggleHighlight(e.target, selectColor);
                obj_point = e.target.gameObject;
                isselect = true;

                obj_point.transform.parent = pointer.transform;
                scanActivity = false;
            }

            if (logSetEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
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

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}