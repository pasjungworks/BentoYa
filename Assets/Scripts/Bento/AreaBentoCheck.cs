using UnityEngine;

public class AreaBentoCheck : MonoBehaviour
{
    public static AreaBentoCheck instance;

    public bool insideBentoArea = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            insideBentoArea = true;  
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            insideBentoArea = false;
        }
    }
}
