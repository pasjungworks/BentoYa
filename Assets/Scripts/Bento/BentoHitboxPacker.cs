using System.Collections.Generic;
using UnityEngine;

public class BentoHitboxPacker : MonoBehaviour
{
    [Header("Distance Settings")]

    public LayerMask foodLayer;
    public LayerMask floorLayer;
    public GameObject ghostPrefab;
    public float dragHeight = 0.5f;

    private GameObject selectedFood;
    private List<GameObject> activeGhosts = new List<GameObject>();
    private List<BentoSlot> currentRequiredSlots = new List<BentoSlot>();

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    public Camera bentoCamera;
    private BentoSlot[] allSlots;
    public bool packAble = false;
   /*
    void Start()
    {
        allSlots = GetComponentsInChildren<BentoSlot>();
        FoodItem[] allFood = Object.FindObjectsByType<FoodItem>(FindObjectsSortMode.None);
        foreach (var food in allFood)
        {
            Rigidbody rb = food.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }
    void Update()
    {
        if (packAble)
        {
            if (Input.GetMouseButtonDown(0)) TryPickup();
            if (selectedFood != null) HandleDrag();
            if (Input.GetMouseButtonUp(0)) DropFood();
        }
    }

    void TryPickup()
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

            selectedFood.GetComponent<Rigidbody>().isKinematic = true;

            ClearOldSlots(selectedFood);

            if (ghostPrefab == null) ghostPrefab = Instantiate(ghostPrefab);
        }
    }

    void HandleDrag()
    {
        Ray ray = bentoCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit floorHit, 100f, floorLayer))
        {
            Vector3 targetPos = floorHit.point + Vector3.up * dragHeight;
            selectedFood.transform.position = Vector3.Lerp(selectedFood.transform.position, targetPos, Time.deltaTime * 25f);
        }

        /*if (Input.GetKeyDown(KeyCode.R))
        {
            selectedFood.transform.Rotate(0, 90, 0, Space.Self);
            //selectedFood.GetComponent<FoodItem>().RotateOffsets();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedFood.transform.Rotate(0, 0, 90, Space.Self);
            //selectedFood.GetComponent<FoodItem>().RotateOffsetsCounter();
        }

        UpdateMultiGridLogic();
    }

    void UpdateMultiGridLogic()
    {
        FoodItem foodData = selectedFood.GetComponent<FoodItem>();
        BentoSlot primarySlot = FindClosestSlot();

        float distanceToBento = primarySlot != null ?
            Vector3.Distance(selectedFood.transform.position, primarySlot.transform.position) : float.MaxValue;

        
        float threshold = 0.8f;

        List<BentoSlot> potentialSlots = new List<BentoSlot>();
        bool allValid = false;

        if (primarySlot != null && distanceToBento < threshold)
        {
            allValid = true;
            foreach (Vector2Int offset in foodData.gridOffsets)
            {
                BentoSlot neighbor = FindSlotAtCoords(primarySlot.gridX + offset.x, primarySlot.gridZ + offset.y);

                if (neighbor == null || (neighbor.isOccupied && neighbor.foodInSlot != selectedFood))
                {
                    allValid = false;
                    break;
                }
                potentialSlots.Add(neighbor);
            }
        }

        if (allValid && potentialSlots.Count == foodData.gridOffsets.Count)
        {
            currentRequiredSlots = potentialSlots;
            ShowMultiGhost(potentialSlots);
        }
        else
        {
            currentRequiredSlots.Clear();
            HideGhost();
        }
    }

    void ShowMultiGhost(List<BentoSlot> slots)
    {
        HideGhost();
        foreach (var slot in slots)
        {
            GameObject g = Instantiate(ghostPrefab, slot.transform.position, slot.transform.rotation);

            g.transform.localScale = slot.transform.localScale * 1.01f;

            activeGhosts.Add(g);
        }
    }

    void HideGhost()
    {
        foreach (var g in activeGhosts) Destroy(g);
        activeGhosts.Clear();
    }

    BentoSlot FindClosestSlot()
    {
        BentoSlot closest = null;
        float closestDist = float.MaxValue;

        foreach (BentoSlot slot in allSlots)
        {
            Vector3 localFoodPos = slot.transform.parent.InverseTransformPoint(selectedFood.transform.position);
            Vector3 localSlotPos = slot.transform.parent.InverseTransformPoint(slot.transform.position);

            float dist = Vector2.Distance(new Vector2(localFoodPos.x, localFoodPos.y),
                                          new Vector2(localSlotPos.x, localSlotPos.y));

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = slot;
            }
        }
        return closest;
    }

    BentoSlot FindSlotAtCoords(int x, int z)
    {
        BentoSlot[] allSlots = Object.FindObjectsByType<BentoSlot>(FindObjectsSortMode.None);
        foreach (var s in allSlots)
        {
            if (s.gridX == x && s.gridZ == z) return s;
        }
        return null;
    }

    void DropFood()
    {
        if (selectedFood == null) return;
        FoodItem foodData = selectedFood.GetComponent<FoodItem>();

        if (currentRequiredSlots != null && currentRequiredSlots.Count > 0)
        {
            Vector3 slotCenterSum = Vector3.zero;
            foreach (var s in currentRequiredSlots)
            {
                slotCenterSum += s.transform.position;

                s.isOccupied = true;
                s.foodInSlot = selectedFood;
            }

            Vector3 targetPosition = slotCenterSum / currentRequiredSlots.Count;

            selectedFood.transform.position = targetPosition;


            selectedFood.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            ClearOldSlots(selectedFood);
            StartCoroutine(ZipBack(selectedFood, foodData.spawnPosition, foodData.spawnRotation));
        }

        selectedFood = null;
        currentRequiredSlots.Clear();
        HideGhost();
    }
    System.Collections.IEnumerator ZipBack(GameObject food, Vector3 targetPos, Quaternion targetRot)
        {
            float elapsed = 0;
            float duration = 0.2f;
            Vector3 startPos = food.transform.position;
            Quaternion startRot = food.transform.rotation;

            while (elapsed < duration)
            {
                food.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                food.transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            food.transform.position = targetPos;
            food.transform.rotation = targetRot;
        }
        void ClearOldSlots(GameObject food)
        {
            BentoSlot[] slots = Object.FindObjectsByType<BentoSlot>(FindObjectsSortMode.None);
            foreach (var s in slots)
            {
                if (s.foodInSlot == food) { s.isOccupied = false; s.foodInSlot = null; }
            }
        }
    */
}
