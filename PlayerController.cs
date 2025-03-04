using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float Deceleration = 20f;
    public bool IsMoving { get; private set; }
    public Vector2 Velocity { get; private set; }
    
    private Rigidbody2D rb;

    public bool CanMove = false;  // Переключаемое состояние (по умолчанию false, т.е. управление не активно)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Если движение заблокировано
        if (!CanMove)
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
            Velocity = input * Speed;
        }
        else
        {
            Velocity = Vector2.Lerp(Velocity, Vector2.zero, Deceleration * Time.deltaTime);
        }

        IsMoving = Velocity.magnitude >= Speed;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Velocity;  // Управление движением в физическом обновлении
    }
}
