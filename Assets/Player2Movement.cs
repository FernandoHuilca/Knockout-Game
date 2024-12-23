using System;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 15f;
    public KeyCode jumpKey = KeyCode.RightShift;
    public LayerMask groundLayer;
    public Transform groundCheck; // Asigna un objeto hijo cerca de los pies del jugador
    public float groundCheckRadius = 0.2f; // Radio del área de detección

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    public Transform weaponHitBox;
    private bool facingRight = true;

    [SerializeField] private AudioClip soundJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Detectar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento horizontal
        float moveX = Input.GetAxis("Horizontal2");
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // Cambiar orientación del sprite según la dirección del movimiento en el eje X
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }

        // Salto con tecla específica
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundsController.Instance.RunSound(soundJump);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        // Cambiar la orientación del sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Invertir la posición X del weaponHitBox
        if (weaponHitBox != null)
        {
            Vector3 localPosition = weaponHitBox.localPosition;
            localPosition.x *= -1;
            weaponHitBox.localPosition = localPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el área de detección del suelo en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
