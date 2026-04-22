using UnityEngine;
using UnityEngine.Video;

public class StopVideo : MonoBehaviour
{
    public VideoPlayer video;

    private void Start()
    {
        video.Stop();
    }
}
