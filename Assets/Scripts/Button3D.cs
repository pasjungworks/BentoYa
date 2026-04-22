using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    [Header("Settings")]
    public UnityEvent onClickAction;

    [Header("Hover Visuals")]
    public float scaleMultiplier = 1.1f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    onClickAction.Invoke();
                }
            }
        }
    }
    private void OnMouseDown()
    {
        if (onClickAction != null)
        {
            onClickAction.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale = originalScale * scaleMultiplier;
    }

    private void OnMouseExit()
    {
        transform.localScale = originalScale;
    }
}
