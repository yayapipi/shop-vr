using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardClicker : MonoBehaviour {
    private int state;
    private EventSystem m_EventSystem;
    private GameObject rayCastObj;
    private GameObject rayCastObj_last;
    private RaycastHit obj;
    private GameObject lastPointerDownObj;
    private List<RaycastResult> raycastResults;
    private PointerEventData pointer;
    private bool hit;

    void Start()
    {
        m_EventSystem = EventSystem.current;
        state = 1;

        pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);
        pointer.button = PointerEventData.InputButton.Left;

        raycastResults = new List<RaycastResult>();
    }

    void Update()
    {
        if (state == 0) //none
        { }
        else if(state == 1) //controller
            RayDetect();
        else if (state == 2)
            RayDetect();
    }

    public void SetState(int toState)
    {
        if (state == 1 && toState == 0)
        {
            //line.SetActive(true);
        }
        else if (state == 0 && toState == 1)
        {
            //line.SetActive(false);
        }
        state = toState;
    }

    private void AutoClicker()
    {
        if (lastPointerDownObj != null)
        {
            if (lastPointerDownObj == rayCastObj)
                ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);

        }
    }

    /* Support hold button, but need to pass pointerEventData argument.*/
    private void RayDetect()
    {
        raycastResults.Clear();
        m_EventSystem.RaycastAll(pointer, raycastResults);
        hit = false;

        //Sort raycast results
        if (raycastResults.Count > 1)
            raycastResults.Sort(RaycastComparer);

        //foreach (RaycastResult h in raycastResults)
        //{
        //    Debug.Log("NAME:" + h.gameObject.name + " ||DISTANCE:" + h.distance);
        //}

        //obj filter
        foreach (RaycastResult h in raycastResults)
        {
            if (h.gameObject.GetComponent<Selectable>())
            {
                hit = true;
                rayCastObj = h.gameObject;
                break;
            }
            if (h.gameObject.name == "mask")
            {
                break;
            }
        }

        if (hit)
        {
            if (rayCastObj != rayCastObj_last)
            {
                if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
                    ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);

                ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerEnterHandler);
            }

            //button down
            if (Input.GetMouseButtonDown(0))
            {
                ExecuteEvents.Execute(rayCastObj, pointer, ExecuteEvents.pointerDownHandler);
                lastPointerDownObj = rayCastObj;

                //AutoClick
                InvokeRepeating("AutoClicker", 1.5f, 0.1f);
            }

            rayCastObj_last = rayCastObj;
        }
        else
        {
            if (rayCastObj_last && rayCastObj_last.GetComponent<Selectable>())
                ExecuteEvents.Execute(rayCastObj_last, pointer, ExecuteEvents.pointerExitHandler);

            rayCastObj = null;
            rayCastObj_last = null;
        }

        //button up
        if (Input.GetMouseButtonUp(0) && lastPointerDownObj != null)
        {
            if (lastPointerDownObj == rayCastObj)
                ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);

            ExecuteEvents.Execute(lastPointerDownObj, pointer, ExecuteEvents.pointerUpHandler);

            //StopAutoClick
            CancelInvoke("AutoClicker");
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
