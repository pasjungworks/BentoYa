using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Play_MiniGame : MonoBehaviour
{
    public static Play_MiniGame instance;

    public Spawner_Minigame Spawner_Notes;
    public Note _note;

    [Header("Score Tiers")]
    public float perfect = 0.2f;
    public float good = 0.5f;
    public float ok = 0.8f;
    public int perfectScore;
    public int goodScore;
    public int okScore;


    [Header("Score")]
    public float score = 0;
    public float combo = 0;
    public int maxCombo;
    public float scorePerSec;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ratingText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI perFectText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI badText;
    public TextMeshProUGUI missText;

    public int perFect = 0;
    public int Good = 0;
    public int bad = 0;
    public int miss = 0;

    public TextMeshProUGUI cookReview;
    public Transform _TargetPoint;

    [Header("FindNode")]
    [SerializeField] private GameObject closestNotes;
    [SerializeField] private float shortestDistance;

    public string raTing;
    public bool canPlay = false;
    public bool isBeingHold = false;
    public bool canBeScore = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Don't use DontDestroyOnLoad yet unless you need it
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
    private void Update()
    {
        //if(_note != null) Debug.Log(canBeScore);

        if (!isBeingHold)
        {
            FindNote();
        }

        if (!canPlay) return;

        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            _Play_MiniGame();
        }
        if (_note != null && _note.isLongNote && _note.isShrinking && canBeScore)
        {
            // If the note is at the target and the player is holding the right key
            if (Input.GetKey(_note.noteKey))
            {
                isBeingHold = true;
                //Debug.Log("Scoring Hold!");
                //ScoreLongNote(scorePerSec * Time.deltaTime);
            }
            else
            {
                if (isBeingHold)
                {
                    isBeingHold = false;
                    ResetCombo();
                }
            }
        }
    }
    public void _Play_MiniGame()
    {
        
        if (closestNotes != null && shortestDistance < ok)
        {
            Note targetNote = closestNotes.GetComponent<Note>();

            if (Input.GetKeyDown(targetNote.noteKey))
            {
                //Debug.Log("work");
                _note = targetNote;
                if (!_note.isLongNote)
                {
                    ScoreTapNote(shortestDistance);
                    Destroy(closestNotes);
                    _note = null;
                }
                else
                {
                    if(_note.isShrinking & isBeingHold == false)
                    {
                        canBeScore = false;
                    }
                    else
                    {
                        canBeScore = true;
                        CheckDistance(shortestDistance);
                    }
                }
                //Debug.Log(shortestDistance+"Short");
                //Debug.Log(_TargetPoint.position);
                //Debug.Log(closestNotes.transform.position);
            }
            else
            {
                //Debug.Log("Miss");
                ResetCombo();

            }
        }
        else
        {
            ResetCombo();    
        }
    }
    void ScoreTapNote(float distance)
    {
        CheckDistance(distance);
    }

    void CheckDistance(float distance)
    {
        if (_note != null && _note.isLocked)
        {
            raTing = "Miss!";
            ResetCombo();
            return;
        }
        Debug.Log("call4");
        int pointsAdded = 0;

        if (distance <= perfect)
        {
            pointsAdded = perfectScore;
            raTing = "Perfect!";
            IncreaseCombo();
            UpdateUI();

        }
        else if (distance <= good)
        {
            pointsAdded = goodScore;
            raTing = "Good!";
            IncreaseCombo();
            UpdateUI();

        }
        else if (distance <= ok)
        {
            pointsAdded = okScore;
            raTing = "Bad!";
            ResetCombo();
        }
        else
        {
            raTing = "Miss!";
            ResetCombo();
        }
        scorePerSec = pointsAdded;

        //float multiplier = Mathf.Min(1 + combo / 10, 4);
        score += pointsAdded;
        //Debug.Log((rating + (distance * multiplier)) + " combo : " + combo);
    }
    public void ScoreLongNote(float amount)
    {
        score += amount;
        //Debug.Log("call1");

    }
    public void FinishLongNote(bool success)
    {
        isBeingHold = false;
        if(success)
        {
            UpdateUI();
        }
        else
        {
            ResetCombo();
            UpdateUI();
        }
        
    }
    public void IncreaseCombo()
    {
        combo++;
    }
    public void ResetCombo()
    {
        //combo = 0;
        UpdateUI();

    }

    

    public void UpdateUI()
    {
        //Debug.Log("call");
        if (scoreText   != null) scoreText.text = "Score: " + score.ToString("F0");
        if(comboText   != null) comboText.text = "Combo: " + combo.ToString("F0");
        if (cookReview != null) cookReview.text = "Score:" + score.ToString("F0");
        if (ratingText != null) ratingText.text = raTing;
        if(raTing == "Perfect!")
        {
            perFect++;
            perFectText.text = perFect.ToString();
        }
        else if(raTing == "Good!")
        {
            Good++;
            goodText.text = Good.ToString();
        }
        else if(raTing == "Bad!")
        {
            bad++;
            badText.text = bad.ToString();
        }
        else
        {
            miss++;
            missText.text = miss.ToString();
        }
    }
    void FindNote()
    {
        GameObject[] activatesNotes = GameObject.FindGameObjectsWithTag("Note");
        closestNotes = null;
        shortestDistance = Mathf.Infinity;

        foreach (GameObject note in activatesNotes)
        {
            float distance = Vector3.Distance(note.transform.position, _TargetPoint.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestNotes = note;
            }
        }
        if (closestNotes == null) return;
        _note = closestNotes.GetComponent<Note>();
    }
}
