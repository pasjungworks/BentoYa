using UnityEngine;
using UnityEngine.UI;
public class BentoItemUI : MonoBehaviour
{
    public string itemID; // Give each item a unique name (e.g. "Broccoli")
    public bool isUnlocked = false;

    [Header("Visuals")]
    public CanvasGroup canvasGroup; // Drag the button's Canvas Group here
    public Button buttonComponent;

    public void SetLockedState(bool unlocked)
    {
        isUnlocked = unlocked;
        buttonComponent.interactable = unlocked; // Disables clicking
        canvasGroup.alpha = unlocked ? 1f : 0.4f; // Gray out if locked (40% opacity)
    }
}
