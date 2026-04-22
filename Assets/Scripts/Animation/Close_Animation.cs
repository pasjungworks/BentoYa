using UnityEngine;
using UnityEngine.Video;

public class Close_Animation : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvasToDisable;

    void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video Finished! Hiding Canvas.");

        if (canvasToDisable != null)
        {
            canvasToDisable.SetActive(false);
        }
    }
}
