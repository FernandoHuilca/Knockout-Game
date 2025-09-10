using System;
using UnityEngine;

public class AlienAttack : MonoBehaviour
{
    [Header("Attack values")]
    [SerializeField] private float attack1Value;
    [SerializeField] private float attack2Value;
    [SerializeField] private float attackRange; // Rango en el que se pueden detectar jugadores enemigos

    [SerializeField] private float attack1ValueToShield;
    [SerializeField] private float attack2ValueToShield;

    [Header("Attack time settings")]
    [SerializeField] private float attackRate = 1f; // Tasa de ataque: número de ataques por segundo permitidos
    [SerializeField] private float waitingTimeAttack1; // Tiempo de espera entre golpes
    [SerializeField] private float waitingTimeAttack2; // Tiempo de espera entre patadas
    [SerializeField] private float nexAttackTime = 0f; // Acumulador del tiempo de espera para el próximo ataque

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform weaponHitBox; // Posición donde se verificará el impacto de los ataques
    [SerializeField] private AudioClip soundAttack1;
    [SerializeField] private AudioClip soundAttack2;
    [SerializeField] private string ownTag;

    [Header("Scripts")]
    [SerializeField] private AlienSpecialAttack specialAttack;
    [SerializeField] private UserConfiguration userConfiguration;

    
    private void OnValidate() // Método necesario para usar hijos del GameObject en el editor
    {
        if (weaponHitBox != null)
        {
            return;
        }
        weaponHitBox = transform.Find("WeaponHitBox");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        ownTag = gameObject.tag;

        specialAttack = GetComponent<AlienSpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();
    }

    void Update()
    {
        if (Time.time < nexAttackTime) // Solo permite ataques si ha pasado suficiente tiempo desde el último ataque
        {
            return;
        }

        if (Input.GetKeyDown(userConfiguration.getHitKey()))
        {
            performAttack(soundAttack1, "attack1", attack1Value, attack1ValueToShield, waitingTimeAttack1);
            return;
        }
        
        if (Input.GetKeyDown(userConfiguration.getKickKey()))
        {
            performAttack(soundAttack2, "attack2", attack2Value, attack2ValueToShield, waitingTimeAttack2);
            return;
        }
        
        if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
        {
            specialAttack.useSpecialAttack();
            
        }
    }

    private void performAttack(AudioClip soundAttack, string nameAttackAnimator, float attackValue, float attackValueToShield, float waitingTimeAttack)
    {
        SoundsController.Instance.RunSound(soundAttack);
        animator.SetTrigger(nameAttackAnimator);
        applyDamageToEnemies(attackValue, attackValueToShield); // Aplica daño a los enemigos detectados
        //applyDamageToEnemies(attackValue, attackValueToShield, weaponHitBox.position, attackRange, gameObject.tag);
        
        nexAttackTime = Time.time + waitingTimeAttack / attackRate;
    }

    private void applyDamageToEnemies(float attackValue, float attackValueToShield)
    {
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);

        foreach (Collider2D playerEnemy in hitOtherPlayers) // Aplica daño a cada enemigo detectado
        {
            manageDamage(playerEnemy, attackValue, attackValueToShield);
        }
    }

    public void manageDamage(Collider2D playerEnemy, float attackValue, float attackValueToShield)
    {
        Damageable damageable = playerEnemy.GetComponent<Damageable>();
        Shieldable shieldable = playerEnemy.GetComponent<Shieldable>();

        if (damageable == null || gameObject.CompareTag(playerEnemy.tag))
        {
            return;
        }

        if (shieldable == null || !shieldable.IsShieldActive())
        {
            
            if(damageable == null)
            {
                return;
            }
            damageable.decreaseLife(attackValue);
            specialAttack.increaseCharge(attackValue);
            return;
        }

        if (shieldable != null && shieldable.IsShieldActive())
        {
            shieldable.decreaseShieldCapacity(attackValueToShield);
            return;
        }
    }

    private void OnDrawGizmosSelected() // Dibuja un Gizmo para visualizar el área de ataque en la escena
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Color del Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Área circular del rango de ataque
    }
}
