using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Button_1 : MonoBehaviour
{
    
    public string SceneToLoad;
    public TMP_Text TextMeshPro;
    public GameObject reViewUI;
    public TextMeshProUGUI score_text;
    public string level;

    public TextMeshProUGUI numLevel_text;
    public string level_Num;

    public Image[] starIcons;
    public Sprite fillStar;
    public Sprite emptyStar;
    private void Awake()
    {
        if (reViewUI == null) return;

        reViewUI.SetActive(false);

    }
    public void OnMouseDown()
    {
        if (SceneToLoad != null)
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }
    public void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
    public void OnMouseExit() 
    {
        GetComponent<Renderer>().material.color= Color.white;
    }
    public void OnClose()
    {
        TextMeshPro.gameObject.SetActive(false);
    }
    public void OpenReview()
    {
        PlayerData data = SaveSystem.Load();
        reViewUI.SetActive(true);
        int _level = data.ClearedLevel.IndexOf(level);

        int star = 0;

        numLevel_text.text ="Level : " + level_Num;

        if (_level != -1)
        {
            score_text.text ="Highest SCORE : "+ data.levelHighestScore[_level].ToString();
        }
        else
        {
            score_text.text = "Highest SCORE : 0";
        }
        if(star != -1)
        {
            star = data.levelMaxStars[_level];
        }

        for (int i = 0; i < starIcons.Length; i++)
        {
            if(i <star)
            {
                starIcons[i].sprite = fillStar;
            }
            else
            {
                starIcons[i].sprite = emptyStar;
            }
        }
    }
    public void OnCloseReview()
    {
        reViewUI?.SetActive(false);
    }
}
