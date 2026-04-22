using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    public float gameSpeed1;
    public float gameSpeed2;
    public float gameSpeed3;

    public void ResetSpeed()
    {
        Time.timeScale = 1.0f;
    }
    public void ChangeSpeed1()
    {
        Time.timeScale = gameSpeed1;
    }
    public void ChangeSpeed2()
    {
        Time.timeScale = gameSpeed2;

    }
    public void ChangeSpeed3()
    {
        Time.timeScale = gameSpeed3;

    }
}
