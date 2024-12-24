using UnityEngine;

public class Player1ShieldLogic : MonoBehaviour
{
    [Header("Shield Components")]
    public CircleCollider2D circleCollider2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;

    [Header("Shield Settings")]
    public float shieldDuration = 5f; // Duración del escudo (puedes usarla más adelante)

    private Rigidbody2D rb;
    private bool isShieldActive = false;

    private MonoBehaviour[] scriptsToDisable;

    private RigidbodyConstraints2D originalConstraints;

    private int maxHits = 3;

    public int getMaxHits()
    {
        return maxHits;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Guarda las restricciones originales del Rigidbody
        originalConstraints = rb.constraints;

        scriptsToDisable = new MonoBehaviour[]
        {
        GetComponent<Player1Movement>(),
        GetComponent<PlatformScript>(),
        GetComponent<Player1Health>(),
        GetComponent<Player1AttackLogic>()
        };
    }

    private void ToggleShield()
    {
        isShieldActive = !isShieldActive;

        // Activar/Desactivar componentes del escudo
        circleCollider2D.enabled = isShieldActive;
        boxCollider2D.enabled = isShieldActive;
        spriteRenderer.enabled = isShieldActive;

        // Restringir movimiento en X y congelar rotación
        if (isShieldActive)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            Debug.Log("Shield Activated");
        }
        else
        {
            // Restaura las restricciones originales
            rb.constraints = originalConstraints;

            // Corrige ligeramente la posición para forzar el recalculo de colisiones
            rb.position = new Vector2(rb.position.x, rb.position.y + 0.01f);

            Debug.Log("Shield Deactivated");
        }

        // Activar/Desactivar scripts
        foreach (var script in scriptsToDisable)
        {
            script.enabled = !isShieldActive;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleShield();
        }
    }


    /// <summary>
    /// Devuelve si el escudo está activo.
    /// </summary>
    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}
