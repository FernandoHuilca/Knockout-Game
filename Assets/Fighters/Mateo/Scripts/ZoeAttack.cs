using System;
using UnityEngine;

public class ZoeAttack : MonoBehaviour
{

    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public Transform weaponHitBox; // Posición donde se verificará el impacto de las armas
    public float attackRange; // Rango en el que se pueden detectar jugadores enemigos

    // Valores de daño para diferentes ataques
    public float hitDamage;
    public float kickDamage;
    

    public float hitDamageToShield;
    public float kickDamageToShield;
    

    public float attackRate = 1f; // Tasa de ataque: número de ataques por segundo permitidos
    public float waitingTimeHit; // Tiempo de espera entre golpes
    public float waitingTimeKick; // Tiempo de espera entre patadas
    private float nexAttackTime = 0f; // Acumulador del tiempo de espera para el próximo ataque

    //public KeyCode hitKey;
    //public KeyCode kickKey;
    //public KeyCode specialPowerKey;

    private ZoeSpecialAttack specialAttack;
    private UserConfiguration userConfiguration;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundAttack1;
    [SerializeField] private AudioClip soundAttack2;

    string ownTag;

    void Start()
    {
        specialAttack = GetComponent<ZoeSpecialAttack>();
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
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Si se presiona la tecla correspondiente, realiza una patada
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                kick();
                SoundsController.Instance.RunSound(soundAttack2);
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Si se presiona la tecla correspondiente, activa el poder especial
            else if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // Método para realizar el golpe
    void hit()
    {
        animator.SetTrigger("attack1"); // Activa la animación de ataque
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Aplica daño a los enemigos detectados
    }

    // Método para realizar la patada
    private void kick()
    {
        // Activa la animación de ataque
        animator.SetTrigger("attack2"); // DEBERÍA SER DIFRENTE PARA LA ANIMACIÓN DE KICK
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }

    // Método que aplica daño a los enemigos detectados
    public void applyDamageToEnemies(float damage, float damageToShield)
    {
        // Detecta jugadores enemigos dentro del área del "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCapsuleAll(weaponHitBox.position, attackRange, )
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);


        // Aplica daño a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {

            //var health = playerEnemy.GetComponent<AlienHealth>();
            //var shield = playerEnemy.GetComponent<Shield>();
            Damageable damageable = playerEnemy.GetComponent<Damageable>();
            Shieldable shield = playerEnemy.GetComponent<Shieldable>();

            if (damageable != null && gameObject.tag != playerEnemy.tag)
            {
                if (shield == null || !shield.IsShieldActive())
                {
                    damageable.decreaseLife(damage);
                    Debug.Log("We hit " + playerEnemy.name);
                    // Cargar barra de ataque especial con cada golpe acertado
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    shield.decreaseShieldCapacity(damageToShield);
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

    //public void setHitKey(KeyCode hitKey)
    //{
    //    this.hitKey = hitKey;
    //}

    //public void setKickKey(KeyCode kickKey)
    //{
    //    this.kickKey = kickKey;
    //}

    //public void setSpecialPowerKey(KeyCode specialPowerKey)
    //{
    //    this.specialPowerKey = specialPowerKey;
    //}

    //public void setHitDamage(float hitDamageFromPersonaje)
    //{
    //    hitDamage = hitDamageFromPersonaje;
    //}

    //public void setKickDamage(float kickDamageFromPersonaje)
    //{
    //    kickDamage = kickDamageFromPersonaje;
    //}

    //public void setSpecialPowerDamage(float specialPowerDamageFromPersonaje)
    //{
    //    specialPowerDamage = specialPowerDamageFromPersonaje;
    //}

    //public void setHitDamageToShield(float hitDamageToShieldFromPersonaje)
    //{
    //    hitDamageToShield = hitDamageToShieldFromPersonaje;
    //}

    //public void setKickDamageToShield(float kickDamageToShieldFromPersonaje)
    //{
    //    kickDamageToShield = kickDamageToShieldFromPersonaje;
    //}

    //public void setWaitingTimeHit(float waitingTimeHitFromPersonaje)
    //{
    //    waitingTimeHit = waitingTimeHitFromPersonaje;
    //}

    //public void setWaitingTimeKick(float waitingTimeKickFromPersonaje)
    //{
    //    waitingTimeKick = waitingTimeKickFromPersonaje;
    //}

    //public void setAttackRange(float attackRangeFromPersonaje)
    //{
    //    attackRange = attackRangeFromPersonaje;
    //}

    //public void setAttackRate(float attackRateFromPersonaje)
    //{
    //    attackRate = attackRateFromPersonaje;
    //}
}