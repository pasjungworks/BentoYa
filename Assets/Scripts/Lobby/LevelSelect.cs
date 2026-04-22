using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public LevelButtonUI[] levelButtons;

    private void Start()
    {
        RefreshLevelSelect();
    }

    public void RefreshLevelSelect()
    {
        PlayerData data = SaveSystem.Load();

        foreach (LevelButtonUI btn in levelButtons)
        {
            btn.SetupButton(data);
        }
    }
}
