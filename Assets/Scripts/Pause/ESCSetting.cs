using UnityEngine;

public class ESCSetting : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject settingsPanel;

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        if (settingsPanel == null) return;

        bool currentState = settingsPanel.activeSelf;
        settingsPanel.SetActive(!currentState);

        if (settingsPanel.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
