using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; 
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Ищем игрока
    }

    void Update()
    {
        if (player != null)
        {
            // Двигаемся к игроку
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
