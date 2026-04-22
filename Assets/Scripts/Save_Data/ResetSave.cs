using UnityEngine;
using System.Collections.Generic;

public class ResetSave : MonoBehaviour
{
    public void ResetSaveData()
    {
        PlayerData blankData = new PlayerData();

        blankData.ClearedLevel = new List<string>();
        blankData.levelHighestScore = new List<int>();
        blankData.levelMaxStars = new List<int>();
        blankData.unlockedItems = new List<string>();

        blankData.ClearedLevel.Add("Level_1");
        blankData.levelHighestScore.Add(0);
        blankData.levelMaxStars.Add(0);
        blankData.unlockedItems.Add(string.Empty);

        blankData.satisfiesCurrency = 0;
        blankData.totalStars = 0;

        SaveSystem.save(blankData);
        Debug.Log("reset save");

        LevelSelect levelSelect = FindFirstObjectByType<LevelSelect>();
        if(levelSelect != null )
        {
            levelSelect.RefreshLevelSelect();
        }
    }
}
