using UnityEngine;

public class Bullet : DamageToEnemies
{
    [Header("Bullet Settings")]
    [SerializeField] private float life = 3; // Tiempo en segundos que la bala existir� antes de autodestruirse
    [SerializeField] private float damageRadius = 0.5f;

    private float damage;
    private float shieldDamage;
    private string shooterTag;
    private bool hasDealtDamage = false;
    private SpecialAttack shooterSpecialAttack;

    private Rigidbody2D rigidBody2D;

    void Awake()
    {
        Destroy(gameObject, life); // programa la destruccion del objeto bala despu�s de life segundos
    }

    public void Initialize(float damage, float shieldDamage, string shooterTag, SpecialAttack specialAttack)
    {
        this.damage = damage;
        this.shieldDamage = shieldDamage;
        this.shooterTag = shooterTag;
        this.shooterSpecialAttack = specialAttack;

        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.right = rigidBody2D.linearVelocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDealtDamage) return;
        if (other.CompareTag(shooterTag)) return; // No atacar al que disparo

        var damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            // Suscribirse al evento para saber si se aplic� da�o
            DamageToEnemies.instance.OnDamageDealt += HandleBulletDamage;

            DamageToEnemies.instance.applyDamageToEnemies(damage, shieldDamage, transform.position, damageRadius, shooterTag);
            hasDealtDamage = true;
            Destroy(gameObject);
        }
        
    }

    private void HandleBulletDamage(bool wasSuccessful, float totalDamage)
    {
        // Desuscribirse del evento
        DamageToEnemies.instance.OnDamageDealt -= HandleBulletDamage;

        // SI la bala aplic� da�o exitosamente, cargar barra especial del shooter
        if (wasSuccessful && shooterSpecialAttack != null)
        {
            shooterSpecialAttack.increaseCharge(totalDamage);
        }
    }
}
