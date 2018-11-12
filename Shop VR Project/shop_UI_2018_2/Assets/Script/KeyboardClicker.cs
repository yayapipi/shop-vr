﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class KeyboardClicker : MonoBehaviour {
    private MainController mainController;
    private EventSystem m_EventSystem;
    public static GameObject rayCastObj;
    private GameObject dragObj;
    private bool dragging;
    private GameObject rayCastObj_last = null;
    private GameObject lastPointerDownObj;
    private List<RaycastResult> raycastResults;
    private RaycastResult currentRaycast;
    private PointerEventData pointer;
    private bool hit;
    private float timer;

    //Controller events
    public delegate void PointerEventManager(GameObject target);
    public static event PointerEventManager PointerSet;
    public static event PointerEventManager PointerEnter;
    public static event PointerEventManager PointerExit;

    void OnEnable()
    {
        MainController.UIPointerEvent += ChangeState;
        MainController.RTriggerClickDown += ControllerPointerDown;
        MainController.RTriggerClickUp += ControllerPointerUp;
        GameGaze.EyeClose += EyeClose;
        GameGaze.EyeOpen += EyeOpen;
    }

    void OnDisable()
    {
        MainController.UIPointerEvent -= ChangeState;
        MainController.RTriggerClickDown -= ControllerPointerDown;
        MainController.RTriggerClickUp -= ControllerPointerUp;
        GameGaze.EyeClose -= EyeClose;
        GameGaze.EyeOpen -= EyeOpen;
    }

    void Start()
    {
        mainController = MainController.Instance();
        m_EventSystem = EventSystem.current;
        EnablePhysicsRaycaster();

        pointer = new PointerEventData(EventSystem.current);
        pointer.button = PointerEventData.InputButton.Left;
        pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);

        raycastResults = new List<RaycastResult>();
        dragging = false;
    }

    void Update()
    {
        switch (mainController.UIPointerState)
        {
            case 0:
                //none
                break;
            case 1:
                //controller
                RayDetectUI();
                break;
            case 2:
                //eyetracker
                RayDetectUIandObj();
                break;
            case 3:
                //keyboard
                //RayDetectUIandObj();
                testRay();
                MouseDetect();
                break;
        }

        if (dragging)
        {
            ExecuteEvents.Execute(dragObj, pointer, ExecuteEvents.dragHandler);
        }
    }

    private void ChangeState(Camera eventCamera)
    {
        EnablePhysicsRaycaster();
        rayCastObj = null;
        lastPointerDownObj = null;
    }
    
    private void EnablePhysicsRaycaster()
    {
        if (mainController.ControllerPointerCamera)
        {
            //none
        }
        if (mainController.EyetrackerPointerCamera)
        {
            mainController.EyetrackerPointerCamera.GetComponent<PhysicsRaycaster>().enabled = (mainController.UIPointerState == 2);
        }
        if (mainController.KeyboardPointerCamera)
        {
            mainController.KeyboardPointerCamera.GetComponent<PhysicsRaycaster>().enabled = (mainController.UIPointerState == 3);
        }
    }

    //Controller
    private void ControllerPointerDown()
    {
        if (mainController.UIPointerState == 1 && rayCastObj != null)
        {
            if (rayCastObj.GetComponent<Selectable>())
            {
                //UI button down
                ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerDownHandler);

                lastPointerDownObj = rayCastObj;

                //AutoClick
                InvokeRepeating("AutoClicker", 1.5f, 0.1f);
            }/*
            else if (rayCastObj.tag == "Model" && PointerSet != null)
            {
                //obj submit
                PointerSet(rayCastObj);
            }*/
        }
    }

    private void ControllerPointerUp()
    {
        if (mainController.UIPointerState == 1 && lastPointerDownObj != null)
        {
            if (lastPointerDownObj.GetComponent<Selectable>())
            {
                if (lastPointerDownObj == rayCastObj)
                {
                    //UI submit
                    ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);
                }

                //UI button up
                ExecuteEvents.Execute(lastPointerDownObj, pointer, ExecuteEvents.pointerUpHandler);

                //StopAutoClick
                CancelInvoke("AutoClicker");

                lastPointerDownObj = null;
            }
        }
    }

    //eyetracker
    private void EyeClose()
    {
        if (mainController.UIPointerState == 2)
        {
            timer = Time.time;
        }
    }

    private void EyeOpen()
    {
        if (mainController.UIPointerState == 2 && (Time.time - timer < 2))
        {
            StartCoroutine(EyeClickEnumerator());
        }
    }

    private IEnumerator EyeClickEnumerator()
    {
        yield return new WaitForSeconds(0.05f);

        if (mainController.UIPointerState == 2 && rayCastObj != null)
        {
            if (rayCastObj.GetComponent<Selectable>())
            {
                ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerDownHandler);

                ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);

                ExecuteEvents.Execute(lastPointerDownObj, pointer, ExecuteEvents.pointerUpHandler);
            }
            else if (rayCastObj.tag == "Model" && PointerSet != null)
            {
                //obj submit
                PointerSet(rayCastObj);
            }
        }
    }

    //keyboard
    private void MouseDetect()
    {
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (rayCastObj != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (rayCastObj.GetComponent<Selectable>())
                {
                    //UI button down
                    ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerDownHandler);
                    ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.initializePotentialDrag);
                    ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.beginDragHandler);

                    pointer.pointerPressRaycast = currentRaycast;
                    dragObj = rayCastObj;
                    dragging = true;

                    lastPointerDownObj = rayCastObj;

                    //AutoClick
                    InvokeRepeating("AutoClicker", 1.5f, 0.1f);
                }
                else if (rayCastObj.tag == "Model" && PointerSet != null)
                {
                    //obj submit
                    PointerSet(rayCastObj);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (lastPointerDownObj != null)
                {
                    if (lastPointerDownObj.GetComponent<Selectable>())
                    {
                        if (lastPointerDownObj == rayCastObj)
                        {
                            //UI submit
                            ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);
                        }

                        //UI button up
                        ExecuteEvents.Execute(lastPointerDownObj, pointer, ExecuteEvents.pointerUpHandler);
                        ExecuteEvents.Execute(lastPointerDownObj, pointer, ExecuteEvents.endDragHandler);

                        //StopAutoClick
                        CancelInvoke("AutoClicker");

                        lastPointerDownObj = null;
                    }
                }
            }
        }
    }

    //auto clicker
    private void AutoClicker()
    {
        if (lastPointerDownObj != null && lastPointerDownObj == rayCastObj)
        {
            ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);
        }
    }

    /* Support hover button, but need to pass pointerEventData argument.*/
    private void RayDetectUI()
    {
        raycastResults.Clear();
        m_EventSystem.RaycastAll(pointer, raycastResults);
        hit = false;

        //Sort raycast results
        if (raycastResults.Count > 1)
            raycastResults.Sort(RaycastComparer);

        //obj filter
        foreach (RaycastResult h in raycastResults)
        {
            if (h.gameObject.GetComponent<Selectable>())
            {
                hit = true;
                rayCastObj = h.gameObject;
                break;
            }
            if (h.gameObject.name == "mask" || h.gameObject.GetComponent<MeshRenderer>())
            {
                rayCastObj = h.gameObject;
                break;
            }
        }

        if (hit)
        {
            if (rayCastObj != rayCastObj_last)
            {
                if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
                {
                    //UI exit
                    ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);
                }

                if (rayCastObj.GetComponent<Selectable>())
                {
                    //UI enter
                    ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerEnterHandler);
                }
            }
            rayCastObj_last = rayCastObj;
        }
        else
        {
            if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
            {
                //UI exit
                ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);
            }

            rayCastObj = null;
            rayCastObj_last = null;
        }
    }

    /* Support hover button, but need to pass pointerEventData argument.*/
    private void RayDetectUIandObj()
    {
        raycastResults.Clear();
        m_EventSystem.RaycastAll(pointer, raycastResults);
        hit = false;

        //Sort raycast results
        if (raycastResults.Count > 1)
            raycastResults.Sort(RaycastComparer);

        //obj filter
        foreach (RaycastResult h in raycastResults)
        {
            if (h.gameObject.GetComponent<Selectable>() || h.gameObject.tag == "Model")
            {
                hit = true;
                rayCastObj = h.gameObject;
                break;
            }
            if (h.gameObject.name == "mask" || h.gameObject.GetComponent<MeshRenderer>())
            {
                rayCastObj = h.gameObject;
                break;
            }
        }

        if (hit)
        {
            if (rayCastObj != rayCastObj_last)
            {
                if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
                {
                    //UI exit
                    ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);
                }
                else if (rayCastObj_last && rayCastObj_last.tag == "Model" && PointerExit != null)
                {
                    //obj exit
                    PointerExit(rayCastObj_last);
                }

                if(rayCastObj.GetComponent<Selectable>())
                {
                    //UI enter
                    ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerEnterHandler);
                }
                else if (rayCastObj.tag == "Model" && PointerEnter != null)
                {
                    //obj enter
                    PointerEnter(rayCastObj);
                }
            }
            rayCastObj_last = rayCastObj;
        }
        else
        {
            if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
            {
                //UI exit
                ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);
            }
            else if (rayCastObj_last && rayCastObj_last.tag == "Model" && PointerExit != null)
            {
                //obj exit
                PointerExit(rayCastObj_last);
            }

            rayCastObj = null;
            rayCastObj_last = null;
        }
    }

    //implement pointer drag
    private void testRay()
    {
        raycastResults.Clear();
        m_EventSystem.RaycastAll(pointer, raycastResults);
        hit = false;

        //Sort raycast results
        if (raycastResults.Count > 1)
            raycastResults.Sort(RaycastComparer);

        //Debug.Log("==============");

        //obj filter
        foreach (RaycastResult h in raycastResults)
        {
            //Debug.Log(h.gameObject.name);
            currentRaycast = h;

            if (h.gameObject.GetComponent<Selectable>() || h.gameObject.tag == "Model" )
            {
                hit = true;
                break;
            }

            if (h.gameObject.name == "mask" || h.gameObject.GetComponent<MeshRenderer>())
            {
                break;
            }
        }

        rayCastObj = currentRaycast.gameObject;

        if (hit)
        {
            if (rayCastObj != rayCastObj_last)
            {
                if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
                {
                    //UI exit
                    ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);
                }
                else if (rayCastObj_last && rayCastObj_last.tag == "Model" && PointerExit != null)
                {
                    //obj exit
                    PointerExit(rayCastObj_last);
                }

                if (rayCastObj.GetComponent<Selectable>())
                {
                    //UI enter
                    ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerEnterHandler);
                }
                else if (rayCastObj.tag == "Model" && PointerEnter != null)
                {
                    //obj enter
                    PointerEnter(rayCastObj);
                }
            }
            rayCastObj_last = rayCastObj;
        }
        else
        {
            if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
            {
                //UI exit
                ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);
            }
            else if (rayCastObj_last && rayCastObj_last.tag == "Model" && PointerExit != null)
            {
                //obj exit
                PointerExit(rayCastObj_last);
            }

            rayCastObj = null;
            rayCastObj_last = null;
        }
    }

    /* Old version RayDetect */
    /*
    private void RayDetect()
    {
        if (Physics.Raycast(transform.position, transform.forward, out obj))
        {
            rayCastObj = obj.transform.gameObject;

            if (rayCastObj.GetComponent<Selectable>())
            {
                //highlight
                if (rayCastObj != rayCastObj_last)
                    m_EventSystem.SetSelectedGameObject(rayCastObj);

                //click
                if (Input.GetMouseButtonDown(0))
                    //rayCastObj.GetComponent<Button>().onClick.Invoke();
                    ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);
            }
            else
                m_EventSystem.SetSelectedGameObject(null);

            rayCastObj_last = rayCastObj;
        }
        else
        {
            m_EventSystem.SetSelectedGameObject(null);
            rayCastObj_last = null;
        }
    }
    */
    /*
    private virtual DestinationMarkerEventArgs SetDestinationMarkerEvent(float distance, Transform target, RaycastHit raycastHit, Vector3 position, VRTK_ControllerReference controllerReference, bool forceDestinationPosition = false, Quaternion? rotation = null)
    {
        DestinationMarkerEventArgs e;
        e.controllerReference = controllerReference;
        e.distance = distance;
        e.target = target;
        e.raycastHit = raycastHit;
        e.destinationPosition = position;
        e.destinationRotation = rotation;
        e.enableTeleport = enableTeleport;
        e.forceDestinationPosition = forceDestinationPosition;
        return e;
    }
    */

    //Raycast results compare function
    private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
    {
        if (lhs.module != rhs.module)
        {
            if (lhs.module.eventCamera != null && rhs.module.eventCamera != null && lhs.module.eventCamera.depth != rhs.module.eventCamera.depth)
            {
                // need to reverse the standard compareTo
                if (lhs.module.eventCamera.depth < rhs.module.eventCamera.depth)
                    return 1;
                if (lhs.module.eventCamera.depth == rhs.module.eventCamera.depth)
                    return 0;
                return -1;
            }
            if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
                return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
            if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
                return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
        }
        if (lhs.sortingLayer != rhs.sortingLayer)
        {
            // Uses the layer value to properly compare the relative order of the layers.
            var rid = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
            var lid = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
            return rid.CompareTo(lid);
        }
        if (lhs.sortingOrder != rhs.sortingOrder)
            return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
        if (lhs.distance != rhs.distance)
            return lhs.distance.CompareTo(rhs.distance);
        if (lhs.depth != rhs.depth)
            return rhs.depth.CompareTo(lhs.depth);
        return lhs.index.CompareTo(rhs.index);
    }
}
