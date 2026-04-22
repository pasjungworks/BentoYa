using UnityEngine;

public class ToggleMove : MonoBehaviour
{
    public GameObject food;
    public GameObject gizmoContainer;

    public FreePackBento FreePackBento;
    
    /*void Update()
    {
        food = FreePackBento.selectedFood;

        // Toggle Gizmo visibility with W
        if (Input.GetKeyDown(KeyCode.W) && food != null)
        {
            gizmoContainer.SetActive(!gizmoContainer.activeSelf);

            if (gizmoContainer.activeSelf)
            {
                // Snap the gizmo to the food
                gizmoContainer.transform.position = food.transform.position;

                // Tell the arrows which food they are moving
                foreach (var handle in gizmoContainer.GetComponentsInChildren<MoveFood>())
                {
                    handle.objectToMove = food;
                }
            }
        }
    }*/
}
