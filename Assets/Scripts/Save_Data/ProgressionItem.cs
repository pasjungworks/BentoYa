using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionItem : MonoBehaviour
{
    [System.Serializable]
    public class DecorationButton
    {
        public string itemName;
        public Button itemButton;
    }

    [Header("Setup")]
    public List<DecorationButton> decorationButtons;

    [Header("Visual Settings")]
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 0.6f);
    public Color unlockedColor = Color.white;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData data = SaveSystem.Load();

        if (data == null) data = new PlayerData();
        if (data.unlockedItems == null) data.unlockedItems = new List<string>();

        bool changed = false;
        if (!data.unlockedItems.Contains("bento1")) { data.unlockedItems.Add("bento1"); changed = true; }
        if (!data.unlockedItems.Contains("bento2")) { data.unlockedItems.Add("bento2"); changed = true; }
        if (changed) SaveSystem.save(data);

        foreach (var item in decorationButtons)
        {
            bool isUnlocked = data.unlockedItems.Contains(item.itemName);

            item.itemButton.interactable = isUnlocked;

            Image img = item.itemButton.GetComponent<Image>();
            if (img != null)
            {
                img.color = isUnlocked ? unlockedColor : lockedColor;
            }
        }
    }
}
