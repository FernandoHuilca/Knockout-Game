using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Atributos modificables para cada luchador
    public float speed; // Diferente para cada nuevo luchador
    public float jumpForce; // Diferente para cada nuevo luchador

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

    private UserConfiguration userConfiguration;
    private Animator animator;

    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        fighterLayer = LayerMask.NameToLayer("BaseFighter");

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        userConfiguration = GetComponent<UserConfiguration>();
        animator = GetComponent<Animator>();

        InitializeFacingDirection();
    }

    void Update()
    {
        if (DialogueManager.Instance != null)
        {
            if (DialogueManager.Instance.isDialogueActive)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                animator.SetFloat("xVelocity", 0.0f);
                return;
            }
        }

        // Actualizar parámetros del animator en Update (como en el video)
        animator.SetBool("isJumping", !isGrounded);

        HandleMovement();
        HandleJump();
        HandlePlatformDrop();
    }

    void FixedUpdate()
    {
        // Según el video, los parámetros de velocidad se actualizan en FixedUpdate
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y); // Sin Mathf.Abs para detectar caída
        animator.SetBool("isJumping", !isGrounded);
    }

    private void HandleMovement()
    {
        if (gameObject.GetComponent<Shield>().IsShieldActive())
        {
            return;
        }

        // Movimiento horizontal
        float moveX = Input.GetAxis(userConfiguration.getAxis());
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // Cambiar orientación del sprite dependiendo de `facingRight`
        if (moveX > 0 && !userConfiguration.getFacingRight())
        {
            Flip();
        }
        else if (moveX < 0 && userConfiguration.getFacingRight())
        {
            Flip();
        }
    }

    private void HandleJump()
    {
        // Detectar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Input.GetKeyDown(userConfiguration.getJumpKey()) && !gameObject.GetComponent<Shield>().IsShieldActive())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundsController.Instance.RunSound(soundJump);
        }
    }

    private void HandlePlatformDrop()
    {
        if (Input.GetKeyDown(userConfiguration.getDownKey()) && gameObject.layer == fighterLayer)
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }
    }

    private void Flip()
    {
        userConfiguration.setFacingRight(!userConfiguration.getFacingRight());
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void InitializeFacingDirection()
    {
        if (userConfiguration.getFacingRight())
        {
            return;
        }

        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public UserConfiguration getUserConfiguration()
    {
        return userConfiguration;
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }
}