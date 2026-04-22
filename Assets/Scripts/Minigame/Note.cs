using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [Header("Note Settings")]
    public KeyCode noteKey;
    public bool isLongNote = false;
    public float holdDuration = 2f;

    [Header("Bogie Settings")]
    public GameObject bogiePrefab;
    public float bogieSpacing = 50f;
    public List<GameObject> activeBogies = new List<GameObject>();

    [Header("NoteVisuals")]
    public Transform noteTail;

    private float bodyLength;
    public bool isShrinking = false;
    private float holdTimer;

    private bool headScore = false;
    private bool tailScored = false;

    public Move_Minigame Move_Minigame;

    private float noteSpeed;
    private float targetpoint;
    private bool wasReleasedEarly = false;

    public bool isLocked = false;

    public float offSet;

    private RectTransform bodyRect;

    private void Start()
    {

        noteSpeed = Move_Minigame.moveSpeed;

        if (bogiePrefab != null)
            bodyRect = bogiePrefab.GetComponent<RectTransform>();
        targetpoint = Play_MiniGame.instance._TargetPoint.transform.position.x;

        if (isLongNote && bogiePrefab != null)
        {
            int bogieCount = Mathf.RoundToInt(holdDuration);
            bodyLength = bogieCount * bogieSpacing;

            for (int i = 0; i < bogieCount; i++)
            {
                GameObject bogie = Instantiate(bogiePrefab, transform);

                float xPos = (i + 1) * bogieSpacing;

                if (bogie.GetComponent<BogieCheckpoint>() == null)
                    bogie.AddComponent<BogieCheckpoint>();

                bogie.transform.SetAsFirstSibling();
                bogie.transform.localPosition = new Vector3(xPos, 0, 0);
                activeBogies.Add(bogie);
            }
            if (noteTail != null)
            {
                // We spawn a copy of the tail prefab as a child of this Note
                GameObject tailInstance = Instantiate(noteTail.gameObject, transform);

                // Re-assign the reference to the NEW instance in the scene
                noteTail = tailInstance.transform;

                // Place it at the very end (after the last bogie)
                noteTail.localPosition = new Vector3(bodyLength + bogieSpacing, 0, 0);
            }
        }
    }
    //
    private void Update()
    {
        if (Move_Minigame == null || isLocked) return;

        if (!isShrinking)
        {
            if (transform.position.x <= targetpoint)
            {
                if (isLongNote)
                {

                    if (Input.GetKey(noteKey))
                    {
                        StopHeadAtTarget();
                    }
                    else
                    {
                        Debug.Log("Miss - Note Locked because key was not held");
                        isLocked = true;
                        Play_MiniGame.instance.raTing = "Miss!";
                        Play_MiniGame.instance.ResetCombo();
                        Debug.Log("call2");


                        return;
                    }
                }
            }
        }
        else
        {

            // Shrink
            holdTimer += Time.deltaTime;

            if (!Input.GetKey(noteKey))
            {
                wasReleasedEarly = true;
            }



            for (int i = 0; i < activeBogies.Count; i++)
            {

                GameObject bogie = activeBogies[i];
                if (bogie == null) continue;

                // Check for scoring
                BogieCheckpoint cp = bogie.GetComponent<BogieCheckpoint>();
                if (bogie.transform.position.x <= targetpoint && !cp.hasScored)
                {
                    cp.hasScored = true;
                    if (Input.GetKey(noteKey))
                    {
                        Play_MiniGame.instance.IncreaseCombo();
                        Play_MiniGame.instance.ScoreLongNote(Play_MiniGame.instance.scorePerSec);
                        Play_MiniGame.instance.UpdateUI();

                        //Debug.Log("call2");
                    }
                }
            }
            //Debug.Log(holdTimer);
            if (noteTail != null)
            {

                // Use .position (World) because the parent is moving the whole group
                if (noteTail.position.x <= targetpoint && !tailScored)
                {
                    Debug.Log("call3");
                    tailScored = true;
                    if (Input.GetKey(noteKey))
                    {
                        Play_MiniGame.instance.ScoreLongNote(Play_MiniGame.instance.scorePerSec);
                        Play_MiniGame.instance.IncreaseCombo();
                        Play_MiniGame.instance.UpdateUI();

                    }
                    FinishNote();
                }
            }

        }
        if (!isLongNote && transform.position.x <= targetpoint)
        {
            //Debug.Log("call");
            Play_MiniGame.instance.raTing = "Miss!";
            Play_MiniGame.instance.ResetCombo();

            Destroy(this.gameObject);
        }
    }
    void FinishNote()
    {
        if (wasReleasedEarly)
        {
            Play_MiniGame.instance.FinishLongNote(false); // failed
        }
        else
        {
            Play_MiniGame.instance.FinishLongNote(true);  // held until end
        }

        Play_MiniGame.instance.FinishLongNote(this.gameObject);
        Destroy(gameObject);
        isShrinking = false;
    }
    void StopHeadAtTarget()
    {
        isShrinking = true;

        if (!headScore && Input.GetKey(noteKey))
        {
            headScore = true;
            Play_MiniGame.instance.IncreaseCombo();
            //Play_MiniGame.instance.ScoreLongNote(Play_MiniGame.instance.scorePerSec);
            Debug.Log("call1");

        }
    }
}
public class BogieCheckpoint : MonoBehaviour
{
    public bool hasScored = false;
}
