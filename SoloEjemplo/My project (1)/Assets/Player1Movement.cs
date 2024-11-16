using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 19f;
    public KeyCode jumpKey = KeyCode.Space; // Tecla específica para el salto de Player1
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
        float moveX = Input.GetAxis("Horizontal"); // Control con el eje "Horizontal"
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // Cambiar orientación del sprite según la dirección del movimiento en el eje X
        if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Salto con tecla específica
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Detecta si el personaje está tocando el suelo
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
