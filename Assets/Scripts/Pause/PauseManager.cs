using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseUI;
    public GameObject countdownUI;
    public VideoPlayer countdownPlayer;

    [Header("Camera Check")]
    public List<GameObject> gameplayScene;

    private bool isPaused = false;
    private bool isCountDown = false;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !isCountDown)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        ToggleVideos(false);
    }
    public void ResumeGame()
    {
        if (CheckCamera())
        {
            StartCoroutine(UnPauseWithCountDown());
        }
        else UnPauseInstant();
    }
    bool CheckCamera()
    {
        foreach (GameObject sce in gameplayScene)
        {
            if (sce != null && sce.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    public void StartUnPause()
    {
        if (isCountDown) return;
        StartCoroutine(UnPauseWithCountDown());
    }

    IEnumerator UnPauseWithCountDown()
    {
        isCountDown = true;
        pauseUI.SetActive(false);
        countdownUI.SetActive(true);

        RawImage videoImage = countdownUI.GetComponent<RawImage>();
        if(videoImage != null) videoImage.enabled = false;

        countdownPlayer.Prepare();
        countdownPlayer.prepareCompleted += OnCountDownPrepared;
        while(!countdownPlayer.isPrepared)
        {
            yield return null;
        }


        countdownPlayer.Play();

        float duration = (float)countdownPlayer.length;
        yield return new WaitForSecondsRealtime(duration);
        countdownUI.SetActive(false);

        UnPauseInstant();
        isCountDown = false;
    }
    void OnCountDownPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnCountDownPrepared;
    }
    void UnPauseInstant()
    {
        isPaused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        ToggleVideos(true);
    }
    void ToggleVideos(bool resuume)
    {
        VideoPlayer[] players = FindObjectsByType<VideoPlayer>(FindObjectsSortMode.None);
        foreach (VideoPlayer vp in players)
        {
            if(vp == countdownPlayer) continue;
            if (vp.gameObject.activeInHierarchy)
            {
                if (resuume) vp.Play();
                else vp.Play();
            }
        }
    }
}
