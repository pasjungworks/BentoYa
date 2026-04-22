using TMPro;
using UnityEngine;

public class UpdateBestScore : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText1;
    public TextMeshProUGUI bestScoreText2;
    public TextMeshProUGUI bestScoreText3;
    public TextMeshProUGUI bestScoreText4;


    private void Start()
    {
        DisplayScore();
    }
    public void DisplayScore()
    {
        PlayerData data = SaveSystem.Load();

        if (data == null || data.levelHighestScore == null) return;

        if(data.levelHighestScore.Count > 0)
        {
            int score1 = data.levelHighestScore[0];
            bestScoreText1.text = "Best Score : " + score1.ToString();
            //Debug.Log(data.levelHighestScore[0]);
        }
        if (data.levelHighestScore.Count > 1)
        {
            int score2 = data.levelHighestScore[1];
            bestScoreText2.text = "Best Score : " + score2.ToString("F0");
        }
        if (data.levelHighestScore.Count > 2)
        {
            int score3 = data.levelHighestScore[2];
            bestScoreText3.text = "Best Score : " + score3.ToString("F0");
        }
        if (data.levelHighestScore.Count > 3)
        {
            int score4 = data.levelHighestScore[3];
            bestScoreText4.text = "Best Score : " + score4.ToString("F0");
        }
    }


}
