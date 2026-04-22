using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    [SerializeField] float hoverScale = 1.2f;
    [SerializeField] float animationSpeed = 10f;

    private Vector3 targetScale;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
    private void OnDisable()
    {
        transform.localScale = originalScale;
        targetScale = originalScale;
    }
}
