using System.Collections;
using UnityEngine;
using RuntimeGizmos;
using UnityEngine.EventSystems;

public class FreePackBento : MonoBehaviour
{
    public TransformGizmo gizmo;

    public Camera bentoCamera;
    public LayerMask foodLayer;
    public LayerMask floorLayer;
    public LayerMask bottomBoxLayer;
    public LayerMask wallLayer;
    public LayerMask topWallLayer;

    public float dragHeight = 0.5f;
    public float maxMovePerFrame;

    public GameObject selectedFood;
    public GameObject bento_Area;

    public float foodPosY;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public Vector3 lastSafePos;
    public Vector3 currentPos;
    public Vector3 currentScale;
    private Vector3 lastSafeScale = Vector3.zero;

    public float minGroundY;

    public float minScale;
    public float maxScale;

    public bool packAble = false;

    public bool isGizmoPhase = false;

    private float foodWidth;
    private float foodDepth;
    private float foodHeight;

    public Transform bentoParent;

    void Update()
    {
        //Debug.Log(gizmo.mainTargetRoot);
        //Debug.Log(isGizmoPhase);
        //Debug.Log(selectedFood);
        GizmoandFood();

        if (packAble)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (isGizmoPhase)
                {
                    if (gizmo.nearAxis != Axis.None)
                    {
                        return;
                    }

                    Ray ray = bentoCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, 100f, foodLayer))
                    {
                        return;
                    }
                    else
                    {
                        isGizmoPhase = false;
                        gizmo.manualMode = false;
                        gizmo.ClearTargets();

                        selectedFood.transform.SetParent(bentoParent, true);
                        CheckFood.instance.CheckBentoComplete();

                        selectedFood = null;

                        return;
                    }
                }

                PickUp();

            }
            if (selectedFood != null && !isGizmoPhase)
            {
                HandleDrag();
            }

            if (Input.GetMouseButtonUp(0) && !isGizmoPhase)
            {
                DropFood();
            }
        }
    }
    void GizmoandFood()
    {
        if (isGizmoPhase && selectedFood != null)
        {
            FoodItem foodData = selectedFood.GetComponent<FoodItem>();

            Vector3 scaleBeforeFrame = lastSafeScale;
            if (lastSafeScale == Vector3.zero) lastSafeScale = foodData.spawnScale;
            Vector3 currentScale = selectedFood.transform.localScale;

            float minX = foodData.spawnScale.x * minScale;
            float maxX = foodData.spawnScale.x * maxScale;
            float minY = foodData.spawnScale.y * minScale;
            float maxY = foodData.spawnScale.y * maxScale;
            float minZ = foodData.spawnScale.z * minScale;
            float maxZ = foodData.spawnScale.z * maxScale;
            //Clamps
            currentScale.x = Mathf.Clamp(currentScale.x, minX, maxX);
            currentScale.y = Mathf.Clamp(currentScale.y, minY, maxY);
            currentScale.z = Mathf.Clamp(currentScale.z, minZ, maxZ);

            selectedFood.transform.localScale = currentScale;
            //Debug.Log(currentScale);
            //Debug.Log(selectedFood.transform.localScale);

            Mesh foodMesh = selectedFood.GetComponentInChildren<MeshFilter>().sharedMesh;
            Vector3 currentSize = Vector3.Scale(foodMesh.bounds.size, selectedFood.transform.localScale);

            Vector3 scaleCheckExtents = currentSize * 0.48f;
            scaleCheckExtents.y *= 0.8f;
            Vector3 checkCenter = selectedFood.transform.position + (Vector3.up * 0.05f);

            int scaleMask = wallLayer.value | foodLayer.value | topWallLayer.value | bottomBoxLayer.value;
            Collider[] hitColliders = Physics.OverlapBox(checkCenter, scaleCheckExtents, selectedFood.transform.rotation, scaleMask);

            /*
            bool scaleHitSomething = false;

            foreach (var hit in hitColliders)
            {
                if (hit.gameObject != selectedFood.gameObject && !hit.transform.IsChildOf(selectedFood.transform))
                {
                    scaleHitSomething = true;
                    break;
                }
            }
            
            float bottomBeforeScale = selectedFood.transform.position.y;
            if (scaleHitSomething)
            {
                bool hitFloor = false;
                foreach (var hit in hitColliders)
                {
                    if (((1 << hit.gameObject.layer) & bottomBoxLayer.value) != 0)
                    {
                        hitFloor = true;
                        break;
                    }
                }
                if (hitFloor)
                {
                    // INSTEAD OF STOPPING: Push the object up
                    // We calculate where the new bottom WOULD be, and offset the position
                    float newFH = (foodMesh.bounds.size.y * selectedFood.transform.localScale.y) * 0.5f;
                    float bottomAfterScale = selectedFood.transform.position.y - newFH;

                    float sinkAmount = bottomBeforeScale - bottomAfterScale;

                    // Push the transform up by exactly how much it tried to sink
                    Vector3 pushedPos = selectedFood.transform.position;
                    pushedPos.y += sinkAmount;
                    selectedFood.transform.position = pushedPos;

                    // Update variables for the next part of the script
                    currentPos = pushedPos;
                    lastSafePos = pushedPos;
                    foodHeight = newFH;
                }
                else
                {
                    // If it hit a WALL or another FOOD, keep your original "Stop Scaling" behavior
                    selectedFood.transform.localScale = scaleBeforeFrame;
                }
            }

            else
            {
                lastSafeScale = selectedFood.transform.localScale;
            }*/

            foodWidth = currentSize.x * 0.5f;
            foodHeight = currentSize.y * 0.5f;
            foodDepth = currentSize.z * 0.5f;

            currentPos = selectedFood.transform.position;
            if (lastSafePos == Vector3.zero) lastSafePos = currentPos;

            // Speed Limit
            float moveDistance = Vector3.Distance(currentPos, lastSafePos);
            if (moveDistance > maxMovePerFrame)
            {
                Vector3 direction = (currentPos - lastSafePos).normalized;
                currentPos = lastSafePos + (direction * maxMovePerFrame);
            }

            // Overlap Check
            Vector3 posHalfExtents = new Vector3(foodWidth * 0.9f, foodHeight * 0.9f, foodDepth * 0.9f);
            Collider[] overlaps = Physics.OverlapBox(currentPos, posHalfExtents, selectedFood.transform.rotation, foodLayer);

            bool isOverlapping = false;
            foreach (var col in overlaps)
            {
                if (col.gameObject != selectedFood) { isOverlapping = true; break; }
            }

            if (isOverlapping) currentPos = lastSafePos;
            else lastSafePos = currentPos;

            Vector3 rayOrigin = currentPos + Vector3.up * 0.5f;

            ApplyWallConstraints();
        }
    }
    void ApplyWallConstraints()
    {
        if (selectedFood == null) return;

        Vector3 currentPos = selectedFood.transform.position;

        if (lastSafePos == Vector3.zero) lastSafePos = currentPos;
        float moveDistance = Vector3.Distance(currentPos, lastSafePos);
        if (moveDistance > maxMovePerFrame)
        {
            Vector3 direction = (currentPos - lastSafePos).normalized;
            currentPos = lastSafePos + (direction * maxMovePerFrame);

            selectedFood.transform.position = currentPos;
        }
        Mesh foodMesh = selectedFood.GetComponentInChildren<MeshFilter>().sharedMesh;
        Vector3 currentSize = Vector3.Scale(foodMesh.bounds.size, selectedFood.transform.localScale);
        float fW = currentSize.x * 0.5f;
        float fH = currentSize.y * 0.5f;
        float fD = currentSize.z * 0.5f;

        Vector3 foodCheckHalfExtents = new Vector3(fW * 0.9f, fH * 0.9f, fD * 0.9f);
        Collider[] foodOverlaps = Physics.OverlapBox(currentPos, foodCheckHalfExtents, selectedFood.transform.rotation, foodLayer);

        bool touchingOtherFood = false;
        foreach (var col in foodOverlaps)
        {
            if (col.gameObject != selectedFood.gameObject && !col.transform.IsChildOf(selectedFood.transform))
            {
                touchingOtherFood = true;
                break;
            }
        }

        if (touchingOtherFood)
        {
            selectedFood.transform.position = lastSafePos;
            return;
        }

        float shell = 0.05f;

        Vector3 floorRayStart = currentPos + (Vector3.up * 0.3f);
        //  Y AXIS (Floor/Roof)
        // Floor: Shoot Down
        if (Physics.SphereCast(floorRayStart, shell, Vector3.down, out RaycastHit hitFloor, 5f, bottomBoxLayer))
            if (currentPos.y < hitFloor.point.y + fH) currentPos.y = hitFloor.point.y + fH;
        //Debug.Log(hitFloor.point.y);
        //Debug.DrawLine()

        if (Physics.SphereCast(currentPos, shell, Vector3.up, out RaycastHit hitRoof, 5f, topWallLayer))
            if (currentPos.y > hitRoof.point.y - fH) currentPos.y = hitRoof.point.y - fH;

        // X & Z Axis
        float[] heights = { currentPos.y - (fH * 0.8f), currentPos.y, currentPos.y + (fH * 0.8f) };
        foreach (float h in heights)
        {
            Vector3 rayOrigin = new Vector3(currentPos.x, h, currentPos.z);

            if (Physics.SphereCast(rayOrigin, shell, Vector3.right, out RaycastHit hitR, 5f, wallLayer))
                if (currentPos.x > hitR.point.x - fW) currentPos.x = hitR.point.x - fW;

            if (Physics.SphereCast(rayOrigin, shell, Vector3.left, out RaycastHit hitL, 5f, wallLayer))
                if (currentPos.x < hitL.point.x + fW) currentPos.x = hitL.point.x + fW;

            if (Physics.SphereCast(rayOrigin, shell, Vector3.forward, out RaycastHit hitF, 5f, wallLayer))
                if (currentPos.z > hitF.point.z - fD) currentPos.z = hitF.point.z - fD;

            if (Physics.SphereCast(rayOrigin, shell, Vector3.back, out RaycastHit hitB, 5f, wallLayer))
                if (currentPos.z < hitB.point.z + fD) currentPos.z = hitB.point.z + fD;
        }

        selectedFood.transform.position = currentPos;
        lastSafePos = currentPos;
    }
    void PickUp()
    {
        if (bentoCamera == null)
        {
            Debug.LogError("Bento Camera is not assigned in the Inspector!");
            return;
        }
        Ray ray = bentoCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, foodLayer))
        {
            selectedFood = hit.collider.gameObject;

            lastPosition = selectedFood.transform.position;
            lastRotation = selectedFood.transform.rotation;
            if (gizmo != null) gizmo.ClearTargets();

            selectedFood.GetComponent<Rigidbody>().isKinematic = true;
        }
        lastSafeScale = Vector3.zero;
    }
    void HandleDrag()
    {
        Ray ray = bentoCamera.ScreenPointToRay(Input.mousePosition);

        bool hitWall = false;

        bool boxArea = false;

        selectedFood.transform.SetParent(null, true);

        Bounds foodBounds = selectedFood.GetComponent<Collider>().bounds;
        float halfHeight = foodBounds.extents.y;
        Vector3 boxHalfExtents = new Vector3(foodBounds.extents.x * 0.6f, 0.05f, foodBounds.extents.z * 0.6f);

        if (Physics.Raycast(ray, out RaycastHit floorhit, 100f, floorLayer))
        {
            Vector3 targetPos = floorhit.point + Vector3.up * dragHeight;
            selectedFood.transform.position = Vector3.Lerp(selectedFood.transform.position, targetPos, Time.deltaTime * 25f);
            lastSafePos = selectedFood.transform.position;
        }

        int mask = bottomBoxLayer.value | foodLayer.value | wallLayer.value;
        int placeAbleMask = bottomBoxLayer.value | foodLayer.value;
        int finalMask = mask & ~(1 << LayerMask.NameToLayer("Roof"));
        int ignoreMask = LayerMask.GetMask("BottomBox", "UI", "Food", "TopBox");

        Vector3 boxOrigin = selectedFood.transform.position + Vector3.down * 0.2f;
        if (Physics.BoxCast(boxOrigin, boxHalfExtents, Vector3.down, out RaycastHit boxHit, selectedFood.transform.rotation, 100f, mask))
        {
            foodPosY = boxHit.point.y+ halfHeight;
            //Debug.Log("boxHit.collider.name "+boxHit.collider.name);
            //Debug.Log("boxHit.collider.transform.position.y " +boxHit.collider.transform.position.y);
            //Debug.Log("boxHit.point.y " +boxHit.point.y);
            //Debug.Log("halfHeight "+halfHeight);
            //Debug.Log("foodPosY "+foodPosY);

            if (((1 << boxHit.collider.gameObject.layer) & ignoreMask) == 0)
            {
                hitWall = true;
            }
            HitWallBento.instance.hitWallBento = hitWall;
            // Debug visual: shows the "hit" area
        }
        Vector3 bottomBox = selectedFood.transform.position + Vector3.down * 0.2f;
        if (Physics.Raycast(bottomBox, Vector3.down, 100f, bottomBoxLayer))
        {
            boxArea = true;
        }
        AreaBentoCheck.instance.insideBentoArea = boxArea;
        Debug.DrawLine(selectedFood.transform.position, boxHit.point, Color.red);

    }
    void DropFood()
    {
        if (selectedFood == null) return;

        ApplyWallConstraints();

        bool areaCheck = AreaBentoCheck.instance.insideBentoArea;
        bool hitWall = HitWallBento.instance.hitWallBento;

        FoodItem foodData = selectedFood.GetComponent<FoodItem>();

        Vector3 targetPos = selectedFood.transform.position;
        targetPos.y = foodPosY;

        if (areaCheck == true && hitWall == false)
        {
            selectedFood.transform.position = targetPos;
            //Debug.Log("food final :" +targetPos);
            if (gizmo != null)
            {
                gizmo.manualMode = true;
                gizmo.ClearTargets();
                gizmo.AddTarget(selectedFood.transform);
                currentPos = selectedFood.transform.position;
                lastSafePos = currentPos;
                isGizmoPhase = true;
            }

            selectedFood.GetComponent<Rigidbody>().isKinematic = true;
            Bounds b = selectedFood.GetComponentInChildren<Renderer>().bounds;
            foodWidth = b.extents.x;
            foodDepth = b.extents.z;
            foodHeight = b.extents.y;
        }
        else
        {
            StartCoroutine(ZipBack(selectedFood, foodData.spawnPosition, foodData.spawnRotation));
        }
        lastSafeScale = Vector3.zero;

        //lastSafePos = Vector3.zero;
        //lastSafeScale = Vector3.zero;
        CheckFood.instance.CheckBentoComplete();

    }
    IEnumerator ZipBack(GameObject food, Vector3 targetPos, Quaternion targetRot)
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector3 startPos = food.transform.position;
        Quaternion startRot = food.transform.rotation;

        while (elapsed < duration)
        {
            food.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            food.transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return food;
        }
        food.transform.position = targetPos;
        food.transform.rotation = targetRot;
        selectedFood = null;

    }
    public void ResetSelectedFood()
    {
        if (selectedFood == null) return;

        FoodItem foodData = selectedFood.GetComponent<FoodItem>();
        if (foodData != null)
        {
            selectedFood.transform.position = foodData.spawnPosition;
            selectedFood.transform.rotation = foodData.spawnRotation;
            selectedFood.transform.SetParent(foodData.spawnParent, true);
            selectedFood.transform.localScale = foodData.spawnScale;
            //Debug.Log(foodData.spawnScale);
            //Debug.Log(selectedFood.transform.localScale);


            isGizmoPhase = false;
            gizmo.manualMode = false;
            gizmo.ClearTargets();

            lastSafePos = foodData.spawnPosition;
            lastSafeScale = foodData.spawnScale;
            //Debug.Log(selectedFood.transform.localScale);

            selectedFood = null;
        }
    }
}
