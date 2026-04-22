using UnityEngine;

public class OpenSave : MonoBehaviour
{
    public void Opensave()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
}
