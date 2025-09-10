using System;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [Header("Components Settings")]
    [SerializeField] protected Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    [SerializeField] protected Transform weaponHitBox; // Posición donde se verificará el impacto de las armas
    [SerializeField] protected float attackRange; // Rango en el que se pueden detectar jugadores enemigos

    // Valores de daño para diferentes ataques
    [Header("Attack Values")]
    public float attack1Damage;
    public float attack2Damage;
    public float specialPowerDamage;
    
    public float attack1DamageToShield; 
    public float attack2DamageToShield;

    [Header("Attack Settings")]
    public float attackRate = 1f; // Tasa de ataque: número de ataques por segundo permitidos
    public float waitingTimeAttack1; // Tiempo de espera entre golpes
    public float waitingTimeAttack2; // Tiempo de espera entre patadas
    private float nexAttackTime = 0f; // Acumulador del tiempo de espera para el próximo ataque

    //public KeyCode hitKey;
    //public KeyCode kickKey;
    //public KeyCode specialPowerKey;

    [Header("Script")]
    [SerializeField] private SpecialAttack specialAttack; //SpecialAttack specialAttack;
    [SerializeField] private UserConfiguration userConfiguration;

    // Atributos para sonidos
    [Header("Sounds")]
    [SerializeField] private AudioClip soundAttack1;
    [SerializeField] private AudioClip soundAttack2;

    private string ownTag;

    public void Start()
    {
        specialAttack = GetComponent<SpecialAttack>();
        animator = GetComponent<Animator>();
        userConfiguration = GetComponent<UserConfiguration>();
        ownTag = gameObject.tag;
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update se llama una vez por cuadro
    void Update()
    {
        // Solo permite ataques si ha pasado suficiente tiempo desde el último ataque
        if (Time.time >= nexAttackTime)
        {
            // Si se presiona la tecla correspondiente, realiza un golpe
            if (Input.GetKeyDown(userConfiguration.getHitKey()))
            {
                performAttack1();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeAttack1 / attackRate;
            }
            // Si se presiona la tecla correspondiente, realiza una patada
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                performAttack2();
                SoundsController.Instance.RunSound(soundAttack2);
                nexAttackTime = Time.time + waitingTimeAttack2 / attackRate;
            }
            // Si se presiona la tecla correspondiente, activa el poder especial
            else if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // Método para realizar el golpe
    protected abstract void performAttack1();

    // Método para realizar la patada
    protected abstract void performAttack2();

    // Método helper para aplicar daño
    protected void ApplyMeleeDamage(float damage, float shieldDamage)
    {
        // Suscribirse temporalmente al evento de daño
        DamageToEnemies.instance.OnDamageDealt += HandleDamageDealt;

        DamageToEnemies.instance.applyDamageToEnemies(damage, shieldDamage, weaponHitBox.position, attackRange, gameObject.tag);
    }

    private void HandleDamageDealt(bool wasSuccessful, float totalDamage)
    {
        // Desuscribirse del evento
        DamageToEnemies.instance.OnDamageDealt -= HandleDamageDealt;

        // SI se aplicó daño exitosamente, cargar barra especial
        if (wasSuccessful && specialAttack != null)
        {
            specialAttack.increaseCharge(totalDamage);
            Debug.Log($"Special attack charged with: {totalDamage}");
        }
    }

    // Método necesario para usar hijos del GameObject en el editor
    private void OnValidate()
    {
        if (weaponHitBox == null)
        {
            weaponHitBox = transform.Find("WeaponHitBox");
            if (weaponHitBox == null)
            {
                Debug.LogWarning("WeaponHitBox not found. Ensure there is a child GameObject named 'WeaponHitBox'.");
            }
        }
    }

    // Dibuja un Gizmo para visualizar el área de ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Color del Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Área circular del rango de ataque
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public float getAttack2DamageToShield()
    {
        return attack2DamageToShield;
    }

    public float getSpecialPowerDamage()
    {
        return specialPowerDamage;
    }

}
