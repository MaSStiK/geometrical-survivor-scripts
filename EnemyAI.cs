using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool canDamage = true; // Флаг, можно ли наносить урон

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>(); // Получаем компонент здоровья игрока
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDamage)
        {
            playerHealth?.TakeDamage(1); // Наносим 1 урон
            StartCoroutine(DamageCooldown()); // Запускаем задержку перед следующим уроном
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(1f); // Ждём 1 секунду перед следующим уроном
        canDamage = true;
    }
}
