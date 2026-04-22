using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    public VideoPlayer countdownPlayer;

    private bool isPaused = false;
    private bool isCountDown = false;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && !isCountDown)
        {
            if(isPaused) StartUnPause();
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

    public void StartUnPause()
    {
        if (isCountDown) return;
        StartCoroutine(UnPause());
    }
    
    IEnumerator UnPause()
    {
        isCountDown = true;

        pauseUI.SetActive(false);

        countdownPlayer.Play();

        float duration = (float)countdownPlayer.length;
        yield return new WaitForSecondsRealtime(duration);

        isCountDown=false;
        isPaused=false;

        Time.timeScale = 1;
        ToggleVideos(true);
    }
    void ToggleVideos(bool resuume)
    {
        VideoPlayer[] players = FindObjectsByType<VideoPlayer>(FindObjectsSortMode.None);

        foreach (VideoPlayer vp in players)
        {
            if(vp.gameObject.activeInHierarchy)
            {
                if(resuume) vp.Pause();
                else vp.Play();
            }
        }
    }
}
