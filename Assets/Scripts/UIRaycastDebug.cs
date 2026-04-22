using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycastDebug : MonoBehaviour
{
    void Update()
    {
        // Check if the mouse is over a UI element
        if (EventSystem.current == null)
        {
            Debug.LogWarning("Missing EventSystem in scene!");
            return;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count > 0)
        {
            // The first item in the list is the one "blocking" the click
            GameObject topmostObject = results[0].gameObject;
            Debug.Log("Mouse is over: " + topmostObject.name, topmostObject);
        }
    }
}
