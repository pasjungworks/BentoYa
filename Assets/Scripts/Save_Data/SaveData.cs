using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    private string Level_Name;
    public string nextLevelName;
    public LevelInfo LevelInfo;

    private void Awake()
    {
        Level_Name = SceneManager.GetActiveScene().name;

    }
    public void OnLevelComplete(float scoreThisRound, int starsThisRound, int currencyGained)
    {
        int finalScore = (int)scoreThisRound;

        PlayerData data = SaveSystem.Load();

        if (starsThisRound >= 1 && LevelInfo != null)
        {
            foreach (string item in LevelInfo.itemsToUnlock)
            {
                if (!data.unlockedItems.Contains(item))
                {
                    data.unlockedItems.Add(item);
                    Debug.Log("Unlocked item added to save: " + item);
                }
            }
        }

        int index = data.ClearedLevel.IndexOf(Level_Name);
        int previousBest = 0;
        if (index != -1 && index < data.levelSatisfiesCurrencyHighest.Count)
        {
            previousBest = data.levelSatisfiesCurrencyHighest[index];
        }
        if (currencyGained > previousBest)
        {
            data.satisfiesCurrency += (currencyGained - previousBest);
        }

        UpdateLevelData(data, Level_Name, finalScore, starsThisRound, currencyGained);

        if (starsThisRound >= 1 && !string.IsNullOrEmpty(nextLevelName))
        {
            if(!data.ClearedLevel.Contains(nextLevelName))
            {
                data.ClearedLevel.Add(nextLevelName);
                data.levelHighestScore.Add(0);
                data.levelMaxStars.Add(0);
            }
        }

        data.totalStars = 0;
        foreach (int s in data.levelMaxStars)
        {
            data.totalStars += s;
        }

        SaveSystem.save(data);

        Debug.Log("game save");
    }
    void UpdateLevelData(PlayerData data, string levelName, int newScore, int newStars, int newCurrency)
    {
        int index = data.ClearedLevel.IndexOf(levelName);

        if (index == -1)
        {
            data.ClearedLevel.Add(levelName);
            data.levelHighestScore.Add(newScore);
            data.levelMaxStars.Add(newStars);
            data.levelSatisfiesCurrencyHighest.Add(newCurrency);
        }
        else
        {
            while (data.levelHighestScore.Count <= index)
            {
                data.levelHighestScore.Add(0);
            }
            while (data.levelMaxStars.Count <= index)
            {
                data.levelMaxStars.Add(0);
            }
            while (data.levelSatisfiesCurrencyHighest.Count <= index)
            {
                data.levelSatisfiesCurrencyHighest.Add(0);
            }
            if (newScore > data.levelHighestScore[index])
                data.levelHighestScore[index] = newScore;

            if (newStars > data.levelMaxStars[index])
                data.levelMaxStars[index] = newStars;

            if (newCurrency > data.levelSatisfiesCurrencyHighest[index])
                data.levelSatisfiesCurrencyHighest[index] = newCurrency;
        }
    }
}
