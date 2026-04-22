using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string loadScene;

    public void Loadscene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(loadScene);
    }
    public void resetLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
