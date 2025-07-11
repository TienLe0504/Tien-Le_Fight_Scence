using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 startVector;
    public Vector2 endVector;
    public void OnPointerUp(PointerEventData eventData)
    {
        endVector = eventData.position;
        ListenerManager.Instance.TriggerEvent<Vector2>(EventType.Jump, endVector - startVector);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startVector = eventData.position;
    }
}
