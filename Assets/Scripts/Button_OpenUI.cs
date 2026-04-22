using UnityEngine;

public class Button_OpenUI : MonoBehaviour
{
    public GameObject ui;

    public void OpenUI()
    {
        ui.SetActive(!ui.activeSelf) ;
    }
}
