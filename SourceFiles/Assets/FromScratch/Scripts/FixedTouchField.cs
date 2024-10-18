using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
   
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;   
    public bool Pressed;

    Vector2 temp;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TouchDist = temp;
        if (Pressed)
        {

        }
        else
        {
            temp = Vector2.zero;
        }
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Pressed)
        {

            temp = eventData.delta;
        }
        else
        {
            temp = new Vector2();
        }
    }
}