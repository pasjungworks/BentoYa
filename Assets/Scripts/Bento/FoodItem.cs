using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public string foodName;
    public bool isBonusFood;
    public int satisfiesValue;

    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public Vector3 spawnScale;

    public Transform spawnParent;

    //[Header("Shape Definition")]
    //public List<Vector2Int> gridOffsets = new List<Vector2Int> { new Vector2Int(0, 0) };

    void Awake()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        spawnScale    = transform.localScale;
        spawnParent = transform.parent;
    }

    /*public void RotateOffsets()
    {
        for (int i = 0; i < gridOffsets.Count; i++)
        {
            int x = gridOffsets[i].x;
            int y = gridOffsets[i].y;
            gridOffsets[i] = new Vector2Int(y, -x);
        }
    }
    public void RotateOffsetsCounter()
    {
        for (int i = 0; i < gridOffsets.Count; i++)
        {
            int x = gridOffsets[i].x;
            int y = gridOffsets[i].y;

            // Math for -90 degree rotation: (x, y) becomes (-y, x)
            gridOffsets[i] = new Vector2Int(-y, x);
        }
    }*/
}
