using UnityEngine;

public class MoveFood : MonoBehaviour
{
    public enum Axis { X, Y, Z }
    public Axis targetAxis;

    public GameObject objectToMove;

    private Vector3 mouseOffset;
    private float initialMouseZ;

    public FreePackBento FreePackBento;

    /*private void Update()
    {
        objectToMove = FreePackBento.selectedFood;
    }
    void OnMouseDown()
    {

        if (objectToMove == null) return;

        // Find how far the object is from the camera
        initialMouseZ = Camera.main.WorldToScreenPoint(objectToMove.transform.position).z;

        // Calculate the offset between mouse and object position
        mouseOffset = objectToMove.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (objectToMove == null) return;

        Vector3 currentMousePos = GetMouseWorldPos() + mouseOffset;
        Vector3 newPosition = objectToMove.transform.position;

        // Only update the specific axis of this handle
        if (targetAxis == Axis.X) newPosition.x = currentMousePos.x;
        if (targetAxis == Axis.Y) newPosition.y = currentMousePos.y;
        if (targetAxis == Axis.Z) newPosition.z = currentMousePos.z;

        // Apply the move to the food
        objectToMove.transform.position = newPosition;

        // Keep the Gizmo itself attached to the food as it moves
        transform.parent.position = newPosition;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = initialMouseZ;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }*/
}
