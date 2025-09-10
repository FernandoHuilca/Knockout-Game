using UnityEngine;
using System.Collections;

public class AlienShield : MonoBehaviour, Shieldable
{
    [Header("Shield Components")]
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private RigidbodyConstraints2D originalConstraints;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip soundShield;
    [SerializeField] private AudioClip soundAttackToShield;

    [Header("Shield Settings")]
    [SerializeField] private float shieldDuration; // Tiempo de recarga si el escudo se desactiva.
    [SerializeField] private float shieldCapacity = 0; // Capacidad del escudo.
    [SerializeField] private float maxShieldCapacity; // Capacidad máxima del escudo.
    [SerializeField] private float rechargeRate; // Cantidad de recarga por segundo.
    [SerializeField] private bool isShieldActive = false; // Estado del escudo.
    [SerializeField] private bool isRechargingFromZero = false; // Para controlar la recarga tras agotarse.
    [SerializeField] private UserConfiguration userConfiguration;

    [Header("Components to Disable")]
    [SerializeField] private MonoBehaviour AlienSpecialAttack;
    [SerializeField] private MonoBehaviour AlienAttack;
    [SerializeField] private MonoBehaviour AlienHealth;
    [SerializeField] private MonoBehaviour AlienMovement;

    void Start()
    {
        shieldCapacity = maxShieldCapacity;

        Transform shield = transform.Find("Shield");
        boxCollider2D = shield.GetComponent<BoxCollider2D>();
        spriteRenderer = shield.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalConstraints = rb.constraints;
        animator = GetComponent<Animator>();

        AlienSpecialAttack = GetComponent<AlienSpecialAttack>();
        AlienAttack = GetComponent<AlienAttack>();
        AlienHealth = GetComponent<AlienHealth>();
        AlienMovement = GetComponent<AlienMovement>();
        userConfiguration = GetComponent<UserConfiguration>();
    }

    void Update()
    {
        // Activa o desactiva el escudo al presionar la tecla "V", solo si no está recargando desde 0.
        if (Input.GetKeyDown(userConfiguration.getShieldKey()) && !isRechargingFromZero)
        {
            ToggleShield();
        }

        // Recarga el escudo si no está activo y no está recargando desde 0.
        if (!isShieldActive && !isRechargingFromZero && shieldCapacity < maxShieldCapacity)
        {
            RechargeShield();
        }
    }

    private void ToggleShield()
    {
        // Si el escudo está recargando desde 0, no se puede activar.
        if (isRechargingFromZero)
        {
            Debug.Log("Shield is recharging from zero and cannot be activated.");
            return;
        }

        isShieldActive = !isShieldActive;
        SoundsController.Instance.RunSound(soundShield);
        UpdateShieldComponents();
        UpdateScriptStates();
    }

    private void UpdateShieldComponents()
    {
        // Actualiza la visibilidad y colisión del escudo.
        boxCollider2D.enabled = isShieldActive;
        spriteRenderer.enabled = isShieldActive;

        // Restringir movimiento en X y congelar rotación
        if (isShieldActive)
        {
            animator.SetTrigger("shield");
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            return;
        }
        // Restaura las restricciones originales
        rb.constraints = originalConstraints;

        // Corrige ligeramente la posición para forzar el recalculo de colisiones
        rb.position = new Vector2(rb.position.x, rb.position.y + 0.01f);
    }

    private void UpdateScriptStates()
    {
        // Habilita o deshabilita los scripts según el estado del escudo.
        bool isActive = !isShieldActive; // Los scripts están activos cuando el escudo no está activo.
        AlienSpecialAttack.enabled = isActive;
        AlienAttack.enabled = isActive;
        AlienHealth.enabled = isActive;
        AlienMovement.enabled = isActive;
    }

    public void TakeDamage(float damage)
    {
        SoundsController.Instance.RunSound(soundAttackToShield);
        if (!isShieldActive || isRechargingFromZero)
        {
            return;
        }

        shieldCapacity -= damage;
        Debug.Log($"Shield capacity: {shieldCapacity}");

        if (shieldCapacity > 0)
        {
            return;
        }
        shieldCapacity = 0;
        StartCoroutine(DesactivateAndRechargeShieldFromZero());
    }

    private IEnumerator DesactivateAndRechargeShieldFromZero()
    {
        SoundsController.Instance.RunSound(soundShield);
        isRechargingFromZero = true;
        isShieldActive = false;
        UpdateShieldComponents();
        UpdateScriptStates();

        Debug.Log("Shield is fully depleted and recharging...");
        yield return new WaitForSeconds(shieldDuration);

        isRechargingFromZero = false;
        Debug.Log("Shield recharge timer complete. Passive recharge started.");
    }

    private void RechargeShield()
    {
        shieldCapacity += rechargeRate * Time.deltaTime;
        if (shieldCapacity > maxShieldCapacity)
        {
            shieldCapacity = maxShieldCapacity;
        }

        Debug.Log($"Passive Shield Recharge: {shieldCapacity}");
    }

    /// <summary>
    /// Devuelve si el escudo está activo.
    /// </summary>
    public bool IsShieldActive()
    {
        return isShieldActive && !isRechargingFromZero;
    }

    public void decreaseShieldCapacity(float amount)
    {
        TakeDamage(amount);
    }
}
