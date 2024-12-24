using System;
using UnityEngine;

public class FighterAttack : MonoBehaviour
{
    // Referencia al Animator para reproducir animaciones de ataque
    public Animator animator;

    // Posición donde se verificará el impacto de las armas
    public Transform weaponHitBox;

    // Rango en el que se pueden detectar jugadores enemigos
    public float attackRange = 0.5f;

    // Capa que representa al otro jugador (enemigo)
    //public LayerMask otherPlayer;

    // Valores de daño para diferentes ataques
    private float hitDamage = 10f;            // Daño del ataque normal
    private float kickDamage = 5f;          // Daño del ataque de patada
    private float specialPowerDamage = 30f;   // Daño del poder especial

    private float hitDamageToShield = 20f; // Daño al escudo con golpe
    private float kickDamageToShield = 10f; // Daño al escudo con patada

    private float attackRate = 1f; // Tasa de ataque: número de ataques por segundo permitidos
    public float waitingTimeHit = 1.25f; // Tiempo de espera entre golpes
    public float waitingTimeKick = 0.75f; // Tiempo de espera entre patadas
    private float nexAttackTime = 0f; // Acumulador del tiempo de espera para el próximo ataque
    

    public KeyCode hitKey = KeyCode.Alpha1;
    public KeyCode kickKey = KeyCode.Alpha2;
    public KeyCode specialPowerKey = KeyCode.Alpha3;

    private SpecialAttack specialAttack;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundAttack1;

    void Start()
    {
        specialAttack = GetComponent<SpecialAttack>();
        animator = GetComponent<Animator>();
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update se llama una vez por cuadro
    void Update()
    {
        // Solo permite ataques si ha pasado suficiente tiempo desde el último ataque
        if (Time.time >= nexAttackTime)
        {
            // Si se presiona la tecla correspondiente, realiza un golpe
            if (Input.GetKeyDown(hitKey))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Si se presiona la tecla correspondiente, realiza una patada
            else if (Input.GetKeyDown(kickKey))
            {
                kick();
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Si se presiona la tecla correspondiente, activa el poder especial
            else if (Input.GetKeyDown(specialPowerKey))
            {
                specialAttack.UseSpecialAttack();
            }
        }
    }

    // Método para realizar el golpe
    void hit()
    {
        animator.SetTrigger("Attack"); // Activa la animación de ataque
        ApplyDamageToEnemies(hitDamage, hitDamageToShield); // Aplica daño a los enemigos detectados
    }

    // Método para realizar la patada
    private void kick()
    {
        // Activa la animación de ataque
        animator.SetTrigger("Attack"); // DEBERÍA SER DIFRENTE PARA LA ANIMACIÓN DE KICK
        ApplyDamageToEnemies(kickDamage, kickDamageToShield);
    }

    // Método que aplica daño a los enemigos detectados
    private void ApplyDamageToEnemies(float damage, float damageToShield)
    {
        // Detecta jugadores enemigos dentro del área del "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);

        // Aplica daño a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            var health = playerEnemy.GetComponent<FighterHealth>();
            var shield = playerEnemy.GetComponent<FighterShield>();
            

            if (health != null)
            {
                if (shield == null || !shield.IsShieldActive())
                {
                    health.decreaselife(damage);
                    Debug.Log("We hit " + playerEnemy.name);
                    // Cargar barra de ataque especial con cada golpe acertado
                    specialAttack.IncreaseCharge(damage);
                }
                else
                {
                    shield.DecreaseShieldCapacity(damageToShield);
                }
            }
            
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
}
