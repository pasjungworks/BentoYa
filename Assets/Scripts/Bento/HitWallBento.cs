using UnityEngine;

public class HitWallBento : MonoBehaviour
{
    public static HitWallBento instance;

    public bool hitWallBento = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            hitWallBento = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            hitWallBento = false;
        }
    }
}
