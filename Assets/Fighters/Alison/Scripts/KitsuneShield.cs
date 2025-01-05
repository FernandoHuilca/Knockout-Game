using UnityEngine;
using System.Collections;

public class KitsuneShield : MonoBehaviour, Shieldable
{
    [Header("Shield Components")]
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;

    [Header("Shield Settings")]
    public float shieldDuration; // Tiempo de recarga si el escudo se desactiva.
    public float shieldCapacity = 0; // Capacidad del escudo.
    public float maxShieldCapacity; // Capacidad máxima del escudo.
    public float rechargeRate; // Cantidad de recarga por segundo.
    private bool isShieldActive = false; // Estado del escudo.
    private bool isRechargingFromZero = false; // Para controlar la recarga tras agotarse.

    //public KeyCode shieldKey = KeyCode.V;

    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalConstraints;

    private UserConfiguration userConfiguration;

    [Header("Scripts to Disable")]
    public MonoBehaviour specialAttack;
    public MonoBehaviour fighterAttack;
    public MonoBehaviour fighterHealth;
    public MonoBehaviour fighterMovement;

    void Start()
    {
        shieldCapacity = maxShieldCapacity;

        Transform shield = transform.Find("Shield");
        boxCollider2D = shield.GetComponent<BoxCollider2D>();

        spriteRenderer = shield.GetComponent<SpriteRenderer>();

        specialAttack = GetComponent<KitsuneSpecialAttack>();
        fighterAttack = GetComponent<KitsuneAttack>();
        fighterHealth = GetComponent<KitsuneHealth>();
        fighterMovement = GetComponent<KitsuneMovement>();

        rb = GetComponent<Rigidbody2D>();

        // Guarda las restricciones originales del Rigidbody
        originalConstraints = rb.constraints;

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
    }

    private void UpdateScriptStates()
    {
        // Habilita o deshabilita los scripts según el estado del escudo.
        bool isActive = !isShieldActive; // Los scripts están activos cuando el escudo no está activo.
        specialAttack.enabled = isActive;
        fighterAttack.enabled = isActive;
        fighterHealth.enabled = isActive;
        fighterMovement.enabled = isActive;
    }

    public void TakeDamage(float damage)
    {
        if (!isShieldActive || isRechargingFromZero)
            return;

        shieldCapacity -= damage;
        Debug.Log($"Shield capacity: {shieldCapacity}");

        if (shieldCapacity <= 0)
        {
            shieldCapacity = 0;
            StartCoroutine(DeactivateAndRechargeShieldFromZero());
        }
    }

    private IEnumerator DeactivateAndRechargeShieldFromZero()
    {
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
