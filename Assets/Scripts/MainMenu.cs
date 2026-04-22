using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject levelSelect;
    private void Start()
    {
        levelSelect.SetActive(false);
    }
    public void OnStartLevel()
    {
        levelSelect.SetActive(true);
    }

    public void OnCloseSelectLevel()
    {
        levelSelect.SetActive(false);
    }

    public void Option()
    {
        SceneManager.LoadScene("Option");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
