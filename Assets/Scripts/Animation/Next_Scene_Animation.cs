using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Next_Scene_Animation : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject nextSceneName;

    public Transition_bento_Manager bentoManager;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.isLooping = false;

        //videoPlayer.prepareCompleted += OnvideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Prepare();
    }

    void OnvideoPrepared(VideoPlayer source)
    {
        source.Play();
    }
    void OnVideoFinished(VideoPlayer source)
    {
        LoadNextLevel();
    }
    
    void LoadNextLevel()
    {
        Debug.Log("video finish");
        bentoManager.TransitionToCookReview();
    }
    public void SkipVideo()
    {
        if(videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        LoadNextLevel();
    }
    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnvideoPrepared;
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
