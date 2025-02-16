using UnityEngine;

public class StartGame : MonoBehaviour
{
    public float speed = 5f; // Скорость передвижения

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D или стрелки влево/вправо
        float moveY = Input.GetAxis("Vertical");   // W, S или стрелки вверх/вниз

        Vector2 movement = new Vector2(moveX, moveY) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
