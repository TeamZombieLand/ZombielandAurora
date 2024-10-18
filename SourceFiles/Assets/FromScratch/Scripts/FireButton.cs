using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    [SerializeField] bool rotateWithAim;
    [SerializeField] FixedTouchField touchField;

    public void OnDrag(PointerEventData eventData)
    {
        if (rotateWithAim)
            touchField.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(rotateWithAim)
            touchField.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (rotateWithAim)
            touchField.OnPointerUp(eventData);
    }

   
}
