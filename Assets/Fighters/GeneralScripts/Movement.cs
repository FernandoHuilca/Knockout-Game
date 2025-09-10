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

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Detener completamente el movimiento físico
                animator.SetFloat("xVelocity", 0.0f);
                return;
            }
        }
        HandleMovement();
        HandleJump();
        HandlePlatformDrop();
    }

    private void HandleMovement()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxis(userConfiguration.getAxis());
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        //animator.SetFloat("speed", Mathf.Abs(moveX * speed));
        animator.SetFloat("xVelocity", Mathf.Abs(moveX * speed));

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
        bool wasGrounded = isGrounded;
        // Detectar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Si estaba en el aire y ahora toca el suelo, termina la animación de salto
        if (!wasGrounded && isGrounded){
            animator.SetBool("isJumping", false);
        }

        if (isGrounded && Input.GetKeyDown(userConfiguration.getJumpKey()))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded=false;
            animator.SetBool("isJumping", !isGrounded);
            
            //Debug.Log("Jump");
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
        // Visualizar el area de detecci�n del suelo en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void InitializeFacingDirection()
    {
        // Ajustar el sprite y componentes según el valor inicial de facingRight
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
