using System;
using UnityEngine;

public class FighterMovement : MonoBehaviour
{
    // Atributos modificables para cada luchador
    public float speed = 8f; // TODO: Diferente para cada nuevo luchador
    public float jumpForce = 15f; // TODO: Diferente para cada nuevo luchador

    // Atributos modificables en base al player 1 o player 2
    public KeyCode jumpKey; // TODO: Toca ver cómo asignar teclas a cada jugador, en base a si es player 1 o player 2
    private bool facingRight = true; // TODO: Toca ver cómo asignar la orientación inicial de cada luchador, en base a si es player 1 o player 2
    public string axis = "Horizontal2"; // TODO: Toca ver cómo asignar el eje horizontal de cada jugador, en base a si es player 1 o player 2

    // Atributos comunes a todos los luchadores
    public LayerMask groundLayer;
    public Transform groundCheck; // Asigna un objeto hijo cerca de los pies del jugador
    public float groundCheckRadius = 0.2f; // Radio del área de detección
    private bool isGrounded;

    private Rigidbody2D rb;
    
    private SpriteRenderer spriteRenderer;
    public Transform weaponHitBox;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Encuentra el objeto hijo llamado "GroundCheck" en tiempo de ejecución
        groundCheck = transform.Find("GroundCheck");
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck not found. Ensure there is a child GameObject named 'GroundCheck'.");
        }

        // Encuentra el objeto hijo llamado "WeaponHitBox" en tiempo de ejecución
        weaponHitBox = transform.Find("WeaponHitBox");
        if (weaponHitBox == null)
        {
            Debug.LogError("WeaponHitBox not found. Ensure there is a child GameObject named 'WeaponHitBox'.");
        }
    }


    void Update()
    {
        // Detectar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento horizontal
        float moveX = Input.GetAxis(axis);
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
            Debug.Log("Jump");
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

        // Invertir el CapsuleCollider2D
        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (capsuleCollider != null)
        {
            Vector2 offset = capsuleCollider.offset;
            offset.x *= -1; // Invertir la posición del offset en X
            capsuleCollider.offset = offset;
        }
    }

    // Método necesario para usar hijos del GameObject en el editor
    private void OnValidate()
    {
        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
            if (groundCheck == null)
            {
                Debug.LogWarning("GroundCheck not found. Ensure there is a child GameObject named 'GroundCheck'.");
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Visualizar el área de detección del suelo en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
