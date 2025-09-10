using System;
using System.Collections;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    // Atributos modificables para cada luchador
    [SerializeField] private float speed; // Diferente para cada nuevo luchador
    [SerializeField] private float jumpForce; // Diferente para cada nuevo luchador

    [Header("Ground components")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool isGrounded;

    [Header("Components")]
    [SerializeField] private Transform weaponHitBox;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip soundJump;
    [SerializeField] private Animator animator;

    // Atributos para plataformas
    [SerializeField] private GameObject currentOneWayPlatform;
    [SerializeField] private int fighterLayer;
    [SerializeField] private UserConfiguration userConfiguration;

    private void OnValidate()
    {
        groundCheck = transform.Find("GroundCheck");
        weaponHitBox = transform.Find("WeaponHitBox");
        playerCollider = GetComponent<CapsuleCollider2D>();
        fighterLayer = LayerMask.NameToLayer("BaseFighter");
    }

    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        fighterLayer = LayerMask.NameToLayer("BaseFighter");

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        userConfiguration = GetComponent<UserConfiguration>();
        
        initializeFacingDirection();
    }

    void Update()
    {
        if (DialogueManager.Instance != null)
        {
            if (DialogueManager.Instance.isDialogueActive)
            {

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Detener completamente el movimiento físico
                animator.SetFloat("speed", 0.0f);
                return;
            }
        }

        handleMovement();
        handleJump();
        handlePlatformDrop();
    }

    private void handleMovement()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxis(userConfiguration.getAxis());
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        animator.SetFloat("speed", Mathf.Abs(moveX * speed));

        // Cambiar orientación del sprite dependiendo de `facingRight`
        if (moveX > 0 && !userConfiguration.getFacingRight())
        {
            flip();
        }
        else if (moveX < 0 && userConfiguration.getFacingRight())
        {
            flip();
        }
    }


    private void handleJump()
    {
        // Detectar si est? en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded || !Input.GetKeyDown(userConfiguration.getJumpKey()))
        {
            return;
        }
        Debug.Log("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        SoundsController.Instance.RunSound(soundJump);
    }

    private void handlePlatformDrop()
    {
        if (!Input.GetKeyDown(userConfiguration.getDownKey()) || !(gameObject.layer == fighterLayer))
        {
            return;
        }

        if (currentOneWayPlatform == null)
        {
            return;
        }

        StartCoroutine(disablePlatformCollision());
    }

    private void flip()
    {
        userConfiguration.setFacingRight(!userConfiguration.getFacingRight());

        // Cambiar la orientaci?n del sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Invertir la posici?n X del weaponHitBox
        if (weaponHitBox == null)
        {
            return;
        }
        Vector3 localPosition = weaponHitBox.localPosition;
        localPosition.x *= -1;
        weaponHitBox.localPosition = localPosition;

        // Invertir el CapsuleCollider2D
        if (playerCollider == null)
        {
            return;
        }
        Vector2 offset = playerCollider.offset;
        offset.x *= -1;
        playerCollider.offset = offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("OneWayPlatform"))
        {
            return;
        }
        currentOneWayPlatform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("OneWayPlatform"))
        {
            return;
        }
        currentOneWayPlatform = null;
    }

    private IEnumerator disablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el ?rea de detecci?n del suelo en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void initializeFacingDirection()
    {
        // Ajustar el sprite y componentes según el valor inicial de facingRight
        if (userConfiguration.getFacingRight())
        {
            return;
        }

        spriteRenderer.flipX = true;

        if (weaponHitBox == null)
        {
            return;
        }
        Vector3 localPosition = weaponHitBox.localPosition;
        localPosition.x *= -1;
        weaponHitBox.localPosition = localPosition;

        if (playerCollider == null)
        {
            return;
        }
        Vector2 offset = playerCollider.offset;
        offset.x *= -1;
        playerCollider.offset = offset;
    }

    public UserConfiguration getUserConfiguration()
    {
        return userConfiguration;
    }

    public Animator getAnimator()
    {
        return animator;
    }
}
