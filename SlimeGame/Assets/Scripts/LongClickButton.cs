using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pointerDown;
    public float requiredHoldTime;

    public MobileController mc;

    public UnityEvent onLongClick;
    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        mc.startShoot = true;
        Debug.Log("Pulsado");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        mc.endShoot = true;
        Debug.Log("Levantado");
    }
}
