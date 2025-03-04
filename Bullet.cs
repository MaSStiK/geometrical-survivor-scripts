using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxLifetime = 5f; // Время жизни пули
    public float maxDistance = 15f; // Максимальная дистанция полета
    public int damage = 50; // Урон пули

    private Vector3 StartPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        StartPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, maxLifetime); // Удаляем пулю через maxLifetime секунд
    }

    private void Update()
    {
        // Удаляем пулю, если она улетела слишком далеко
        if (Vector3.Distance(StartPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }

        // Поворачиваем пулю в направлении её движения, с учетом поворота на 90 градусов
        if (rb != null && rb.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90); // Добавляем 90 градусов
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Если попали во врага
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>(); // Получаем скрипт врага
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Наносим урон
            }

            Destroy(gameObject); // Удаляем пулю после попадания
        }
    }
}
