using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardClicker : MonoBehaviour {
    private EventSystem m_EventSystem;
    private GameObject rayCastObj;
    private GameObject rayCastObj_last;
    private int state;

    // Use this for initialization
    void Start()
    {
        m_EventSystem = EventSystem.current;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
            RayDetect();
    }

    /*
    public void SetState(int toState)
    {
        if (state == 1 && toState == 0)
            line.SetActive(true);
        else if (state == 0 && toState == 1)
            line.SetActive(false);

        state = toState;
    }*/

    void RayDetect()
    {
        RaycastHit hit;
        //Debug.DrawRay (transform.position, transform.forward * 10, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            rayCastObj = hit.transform.gameObject;

            if (rayCastObj.CompareTag("Button"))
            {
                //highlight
                if (rayCastObj_last == null || rayCastObj_last != rayCastObj)
                    m_EventSystem.SetSelectedGameObject(hit.transform.gameObject);

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
}
