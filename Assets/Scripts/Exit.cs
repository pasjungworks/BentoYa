using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Game is exiting...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
