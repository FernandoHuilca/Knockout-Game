using System;
using System.Collections;
using UnityEngine;

public class FighterMovement : MonoBehaviour
{
    // Atributos modificables para cada luchador
    public float speed; // Diferente para cada nuevo luchador
    public float jumpForce; // Diferente para cada nuevo luchador

    // Atributos modificables en base al player 1 o player 2
    public KeyCode jumpKey; // Asignar teclas a cada jugador
    public KeyCode downKey; // Tecla para pasar plataformas
    public bool facingRight; // Orientaci�n inicial basada en el jugador
    public string axis; // Eje horizontal del jugador

    // Atributos comunes a todos los luchadores
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    private bool isGrounded;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Transform weaponHitBox;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundJump;

    // Atributos para plataformas
    private GameObject currentOneWayPlatform;
    private int fighterLayer;
    private CapsuleCollider2D playerCollider;

    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        fighterLayer = LayerMask.NameToLayer("BaseFighter");

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        InitializeFacingDirection();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandlePlatformDrop();
    }

    private void HandleMovement()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxis(axis);
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // Cambiar orientación del sprite dependiendo de `facingRight`
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }
    }


    private void HandleJump()
    {
        // Detectar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Debug.Log("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundsController.Instance.RunSound(soundJump);
        }
    }

    private void HandlePlatformDrop()
    {
        if (Input.GetKeyDown(downKey) && gameObject.layer == fighterLayer)
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisablePlatformCollision());
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

        // Invertir el CapsuleCollider2D
        if (playerCollider != null)
        {
            Vector2 offset = playerCollider.offset;
            offset.x *= -1;
            playerCollider.offset = offset;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void OnValidate()
    {
        groundCheck = transform.Find("GroundCheck");
        weaponHitBox = transform.Find("WeaponHitBox");
        playerCollider = GetComponent<CapsuleCollider2D>();
        fighterLayer = LayerMask.NameToLayer("BaseFighter");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el �rea de detecci�n del suelo en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void InitializeFacingDirection()
    {
        // Ajustar el sprite y componentes según el valor inicial de facingRight
        if (!facingRight)
        {
            spriteRenderer.flipX = true;

            if (weaponHitBox != null)
            {
                Vector3 localPosition = weaponHitBox.localPosition;
                localPosition.x *= -1;
                weaponHitBox.localPosition = localPosition;
            }

            if (playerCollider != null)
            {
                Vector2 offset = playerCollider.offset;
                offset.x *= -1;
                playerCollider.offset = offset;
            }
        }
    }

    public void setAxis(string axisFromPersonaje)
    {
        axis = axisFromPersonaje;
    }

    public void setUpKey(KeyCode upKey)
    {
        jumpKey = upKey;
    }

    public void setDownKey(KeyCode downKey)
    {
        this.downKey = downKey;
    }

    public void setFacingRight(bool facingRight)
    {
        this.facingRight = facingRight;
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }

    public void setSpeed(float speedFromPersonaje)
    {
        speed = speedFromPersonaje;
    }

    public void setJumpForce(float jumpForceFromPersonaje)
    {
        jumpForce = jumpForceFromPersonaje;
    }

    public void setGroundCheckRadius(float checkRadiusFromPersonaje)
    {
        groundCheckRadius = checkRadiusFromPersonaje;
    }
}
