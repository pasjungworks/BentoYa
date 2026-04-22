using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public string levelName;
    public List<string> itemsToUnlock;
    public PlayerData data;

    public void GrantRewards()
    {
        if (!data.ClearedLevel.Contains(levelName))
            data.ClearedLevel.Add(levelName);

        foreach (string item in itemsToUnlock)
        {
            if (!data.unlockedItems.Contains(item))
                data.unlockedItems.Add(item);
        }
    }
}
