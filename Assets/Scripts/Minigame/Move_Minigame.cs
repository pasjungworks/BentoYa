using UnityEngine;

public class Move_Minigame : MonoBehaviour
{
    public float moveSpeed;
    void Update()
    {
        move();
    }
    void move()
    {
        transform.localPosition += (Vector3.left * moveSpeed * Time.deltaTime);

    }
}
