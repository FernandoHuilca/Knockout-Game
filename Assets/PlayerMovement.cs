using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f; // TODO: Modificar por cada personaje
    public float jumpForce = 15f; // TODO: Modificar por cada personaje

    [Header("Input Settings")]
    public string horizontalAxis = "Horizontal"; // Eje para movimiento horizontal
    public KeyCode jumpKey = KeyCode.Space; // Tecla para salto

    [Header("Ground Detection")]
    public LayerMask groundLayer; // Necesario para cada personaje. No modificable
    public Transform groundCheck; // Asigna un objeto hijo cerca de los pies del jugador (Necesario para cada personaje. No modificable)
    public float groundCheckRadius = 0.2f; // Radio del �rea de detecci�n (Necesario para cada personaje. No modificable)

    [Header("Other Settings")]
    public Transform weaponHitBox; // Caja de colisi�n del arma. TODO: Modificar por cada personaje
    [SerializeField] private AudioClip soundJump; // Sonido del salto. TODO: Modificar por cada personaje

    private Rigidbody2D rb; // TODO: Modificar por cada personaje
    private SpriteRenderer spriteRenderer; // TODO: Modificar por cada personaje
    private bool isGrounded;
    private bool facingRight = true; // TODO: Modificar EN BASE AL USUARIO

    private User1Logic user1Logic;

    void Start()
    {
        //jummpKey = user1Logic.jumpKey;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Detectar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento horizontal
        float moveX = Input.GetAxis(horizontalAxis);
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // Cambiar orientaci�n del sprite seg�n la direcci�n del movimiento
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }

        // Salto con tecla espec�fica
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (soundJump != null)
            {
                SoundsController.Instance.RunSound(soundJump);
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        // Cambiar la orientaci�n del sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Invertir la posici�n X del weaponHitBox
        if (weaponHitBox != null)
        {
            Vector3 localPosition = weaponHitBox.localPosition;
            localPosition.x *= -1;
            weaponHitBox.localPosition = localPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el �rea de detecci�n del suelo en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
