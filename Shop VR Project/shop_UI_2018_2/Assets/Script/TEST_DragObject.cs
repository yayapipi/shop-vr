using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TEST_DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    void Start()
    {
        //Debug.Log("Start");
    }

    public void OnBeginDrag(PointerEventData _EventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData _EventData)
    {
        //Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData _EventData)
    {
       // Debug.Log("OnEndDrag");
    }
}
