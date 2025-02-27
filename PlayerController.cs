using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float deceleration = 40;
    public bool IsMoving { get; private set; }

    public Vector2 velocity { get; private set; }
    private Rigidbody2D rb;

    // Переменная для блокировки управления
    public bool canMove = false;  // Переключаемое состояние (по умолчанию false, т.е. управление не активно)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Если движение заблокировано
        if (!canMove)
        {
            IsMoving = false;
            return;  // Выход из функции, если управление заблокировано
        }

        // Обработка движения игрока, если управление разрешено
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(moveX, moveY);

        if (input.magnitude > 0)
        {
            input.Normalize();
            velocity = input * speed;
        }
        else
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        IsMoving = velocity.magnitude > 0.1f;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = velocity;  // Управление движением в физическом обновлении
    }
}
