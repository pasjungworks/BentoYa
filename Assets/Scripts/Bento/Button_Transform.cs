using RuntimeGizmos;
using UnityEngine;

public class Button_Transform : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform gizmoMenuPanel;
    public Camera bentoCamera;
    public TransformGizmo gizmo;
    public FreePackBento FreePackBento;

    public GameObject selectedFood;

    private void Update()
    {
    }
    private void LateUpdate()
    {
        if(FreePackBento.selectedFood != null && FreePackBento.isGizmoPhase)
        {
            selectedFood = FreePackBento.selectedFood;
            gizmoMenuPanel.gameObject.SetActive(true);
            FollowFoodWith2DUI();
        }
        else
        {
            gizmoMenuPanel.gameObject.SetActive(false);
        }
    }

    void FollowFoodWith2DUI()
    {
        Vector3 worldPos = selectedFood.transform.position + Vector3.up * 0.3f;
        Vector3 screenPos = bentoCamera.WorldToScreenPoint(worldPos);

        if(screenPos.z < 0)
        {
            gizmoMenuPanel.gameObject.SetActive(false);
            return;
        }
        gizmoMenuPanel.position = worldPos;
    }

    public void SetMove()
    {
        gizmo.transformType = TransformType.Move;
    }
    public void SetRotate()
    {
        gizmo.transformType = TransformType.Rotate;
    }
    public void SetScale()
    {
        gizmo.transformType = TransformType.Scale;
    }
    public void ClickReset()
    {
        if (FreePackBento != null)
        {
            FreePackBento.ResetSelectedFood();
        }
    }
}
