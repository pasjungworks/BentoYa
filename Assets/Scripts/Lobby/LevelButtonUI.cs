using UnityEngine;
using UnityEngine.UI;


public class LevelButtonUI : MonoBehaviour
{
    public string LevelName;
    public Button buttonComponent;

    [Header("Visual Marks")]
    public GameObject x;
    public GameObject o;

    public void SetupButton(PlayerData data)
    {
        int index = data.ClearedLevel.IndexOf(LevelName);

        if (index != -1)
        {
            buttonComponent.interactable = true;

            int stars = data.levelMaxStars[index];

            if (stars > 0)
            {
                x.SetActive(true);
                o.SetActive(false);
                Debug.Log("true");
            }
            else
            {
                x.SetActive(false);
                o.SetActive(true);
            }
        }
        else
        {
            buttonComponent.interactable = false;
            x.SetActive(false);
            o.SetActive(false);
        }
    }
}
