using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static ProgressionItem;

public class Result : MonoBehaviour
{
    public Play_MiniGame Play_MiniGame;
    public SaveData SaveData;

    public LevelInfo levelInfo;
    public PlayerData data;

    public GameObject resultCanvas;
    public ProgressionItem progressionItem;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI perfectText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI badText;
    public TextMeshProUGUI missText;

    public TextMeshProUGUI clearText;
    public string levelClearText;

    public RawImage[] starImage;

    public TextMeshProUGUI satisfieCurrency;

    public int starRate1;
    public int starRate2;
    public int starRate3;


    public string loadScene;

    public float _score;
    private float _combo;
    private float _perfect;
    private float _miss;
    private float _good;
    private float _bad;

    private int _star;
    private int satisfiesCurrency;
    private string Level_Name;

    private void Awake()
    {
        Level_Name = SceneManager.GetActiveScene().name;
    }
    public void ShowResult()
    {
        CheckFood.instance.CheckBentoComplete();
        _combo = Play_MiniGame.combo;
        _perfect = Play_MiniGame.perFect;
        _good = Play_MiniGame.Good;
        _bad = Play_MiniGame.bad;
        _miss = Play_MiniGame.miss;

        float bentoScore = 0;
        if (CheckFood.instance != null)
        {
            bentoScore = CheckFood.instance.currentFoodBonusScore;
            satisfiesCurrency = CheckFood.instance.totalSatisfiesEarned;
        }
        _score = Play_MiniGame.score + bentoScore;

        resultCanvas.gameObject.SetActive(true);
        Debug.Log("call");
        UpdateScoreText();

        UpdateStarText();
        if(_score >= starRate3)
        {
            _star = 3;
        }
        else if(_score >= starRate2)
        {
            _star = 2;
        }
        else if(_score >= starRate1)
        {
            _star = 1;
        }
        SaveData.OnLevelComplete(_score, _star, satisfiesCurrency);

    }
    public void CheckWin()
    {
         PlayerData data = SaveSystem.Load();

         if (!data.ClearedLevel.Contains(levelInfo.levelName))
         {
             data.ClearedLevel.Add(levelInfo.levelName);
         }

         foreach (string item in levelInfo.itemsToUnlock)
         {
                //Debug.Log("Unlocking item: " + item);
             if (!data.unlockedItems.Contains(item))
             {
                    //Debug.Log("Adding item to unlocked items222: " + item);
                 data.unlockedItems.Add(item);
             }
         }

            SaveSystem.save(data);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(loadScene);
    }

    public void UpdateScoreText()
    {
        scoreText.text = "score : "+ _score.ToString("f0");
        comboText.text = "Combo : " + _combo.ToString("f0");
        perfectText.text = "" + _perfect.ToString("f0");
        goodText.text = "" + _good.ToString("f0");
        badText.text = "" + _bad.ToString("f0");
        missText.text = "" + _miss.ToString("f0");
        //satisfieCurrency.text = "Satisfies : " + satisfiesCurrency.ToString("f0");
    }
    public void UpdateStarText()
    {
        Debug.Log("call");
        if (_score >= starRate3)
        {
            starImage[0].color = Color.white;
            starImage[1].color = Color.white;
            starImage[2].color = Color.white;
            CheckWin();
        }
        else if (_score >= starRate2)
        {
            starImage[0].color = Color.white;
            starImage[1].color = Color.white;
            starImage[2].color = Color.gray;
            CheckWin();

        }
        else if(_score >= starRate1)
        {
            starImage[0].color = Color.white;
            starImage[1].color = Color.gray;
            starImage[2].color = Color.gray;
            CheckWin();

        }
        else
        {
            starImage[0].color = Color.gray;
            starImage[1].color = Color.gray;
            starImage[2].color = Color.gray;
            clearText.text = levelClearText;
        }
    }
}
