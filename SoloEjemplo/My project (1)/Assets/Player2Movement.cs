using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed = 8f; // Puedes ajustar la velocidad espec�fica de Player2
    public float jumpForce = 15f; // Fuerza de salto espec�fica de Player2
    public KeyCode jumpKey = KeyCode.RightShift; // Tecla espec�fica para el salto de Player2
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxis("Horizontal2"); // Control con el eje "Horizontal2" (configura esto en el Input Manager)
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // Cambiar orientaci�n del sprite seg�n la direcci�n del movimiento en el eje X
        if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Salto con tecla espec�fica
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Detecta si el personaje est� tocando el suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    // Detecta si el personaje deja de tocar el suelo
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
