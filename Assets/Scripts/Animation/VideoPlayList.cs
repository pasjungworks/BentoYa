using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayList : MonoBehaviour
{
    public GameObject[] videoStages;
    private int currentIndex = 0;

    void Start()
    {
        foreach (GameObject go in videoStages)
        {
            go.SetActive(false);
        }
    }

    public void PlayCurrentStage()
    {
        if (currentIndex < videoStages.Length)
        {
            GameObject currentGO = videoStages[currentIndex];

            currentGO.SetActive(true);

            RawImage img = currentGO.GetComponent<RawImage>();
            if (img != null) img.enabled = false;

            VideoPlayer vp = currentGO.GetComponent<VideoPlayer>();

            if (vp != null)
            {
                vp.prepareCompleted += OnVideoPrepared;
                vp.Prepare();
            }
        }
        else
        {
            Debug.Log("All videos finished!");
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnVideoPrepared;

        RawImage img = videoStages[currentIndex].GetComponent<RawImage>();
        if (img != null) img.enabled = true;

        vp.Play();
        vp.loopPointReached += OnStageFinished;
    }

    void OnStageFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnStageFinished;

        if (vp.targetTexture != null)
        {
            vp.targetTexture.Release();
        }

        videoStages[currentIndex].SetActive(false);
        currentIndex++;
        PlayCurrentStage();
    }
}
