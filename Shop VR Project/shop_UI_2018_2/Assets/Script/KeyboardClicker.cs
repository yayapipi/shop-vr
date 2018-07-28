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
        state = 0;

        pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);
        pointer.button = PointerEventData.InputButton.Left;

        raycastResults = new List<RaycastResult>();
    }

    void Update()
    {
        if (state == 0)
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

        //obj filter
        foreach (RaycastResult h in raycastResults)
        {
            //Debug.Log("NAME:" + h.gameObject.name);
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
            ExecuteEvents.Execute(lastPointerDownObj, pointer, ExecuteEvents.pointerUpHandler);

            if (lastPointerDownObj == rayCastObj)
                ExecuteEvents.Execute(rayCastObj, new BaseEventData(m_EventSystem), ExecuteEvents.submitHandler);

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
}
