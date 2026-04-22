using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct NoteData
{
    public int noteID;
    public float delayAfter;
}
public class Event_After : MonoBehaviour
{
    public int tag_Event;
    public Spawner_Minigame Spawner_Minigame;
    public Play_MiniGame Play_MiniGame;
    public Transition_bento_Manager Transition_bento_Manager;

    public VideoPlayList VideoPlayList;
    public GameObject _spawnNote;

    public GameObject previewCook;
    public TextMeshProUGUI _countDownText;

    public List<NoteData> noteOrder = new List<NoteData>();
    private void Awake()
    {
        previewCook.SetActive(false);
    }
    public void Event_Dialogue()
    {
        //Time.timeScale = 5f;
        switch (tag_Event)
        {
            case 0:
                //Debug.Log("call");
                Play_MiniGame.canPlay = true;
                StartCoroutine(StartEvent());
                break;

        }
    }
    IEnumerator StartEvent()
    {

        Transition_bento_Manager.TransitionToMinigame();
        yield return new WaitForSeconds(1.5f);
        VideoPlayList.PlayCurrentStage();


        yield return StartCoroutine(SpawnNote());

    }
    public void OnTransitionBento()
    {
        Transition_bento_Manager.TransitionToBento();
    }
    IEnumerator SpawnNote()
    {
        for (int i = 0; i < noteOrder.Count; i++)
        {
            // 1. Get the data for the current note
            NoteData currentData = noteOrder[i];

            // 2. Spawn the note using the ID
            GameObject currentNoteObject = Spawner_Minigame.SpawnByIndex(currentData.noteID);

            // 3. WAIT for the specific delay requested for THIS note
            // This is where your designer's specific timing happens!
            yield return new WaitForSeconds(currentData.delayAfter);
        }

        // Wait for the song to finish after all notes are spawned
        yield return new WaitForSeconds(10f);
    }
    IEnumerator CountDown()
    {
        _countDownText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);

        int count = 3;
        while (count > 0)
        {
            _countDownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        _countDownText.gameObject.SetActive(false);
        yield return null;
    }
}
