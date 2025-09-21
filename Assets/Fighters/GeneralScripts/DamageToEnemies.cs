using UnityEngine;

public class DamageToEnemies : MonoBehaviour
{

    public static DamageToEnemies instance { get; private set; }

    // Evento para notificar cuando se aplica daño exitosamente
    /*
     * ´System.Action<bool, float>´ es un delegate (delegado) que puede almacenar referencias a métodos que no retornan nada (void) o 
     * reciben dos parámetros: un bool y un float. Funciona como una "lista de métodos" que se pueden ejecutar cuando ocurre un evento.
    */
    public System.Action<bool, float> OnDamageDealt;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método que aplica daño a los enemigos detectados
    public void applyDamageToEnemies(float damage, float damageToShield, Vector2 position, float attackRange, string tag)
    {
        // Detecta jugadores enemigos dentro del área del "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCapsuleAll(weaponHitBox.position, attackRange, )
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(position, attackRange);
        bool damageDealt = false;
        float totalDamageDealt = 0f;

        // Aplica daño a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            // No atacarse a sí mismo
            if (playerEnemy.CompareTag(tag)) continue;

            //var health = playerEnemy.GetComponent<Health>();
            //var shield = playerEnemy.GetComponent<Shield>();
            Damageable damageable = playerEnemy.GetComponent<Damageable>();
            Shieldable shieldable = playerEnemy.GetComponent<Shieldable>();

            if (damageable != null)
            {
                if (shieldable == null || !shieldable.IsShieldActive())
                {
                    damageable.decreaseLife(damage);
                    damageDealt = true;
                    totalDamageDealt += damage;
                    //Debug.Log($"Damage dealt to {playerEnemy.name}: {damage}");
                    // Cargar barra de ataque especial con cada golpe acertado
                    //specialAttack.increaseCharge(damage);
                }
                else
                {
                    shieldable.decreaseShieldCapacity(damageToShield);
                    Debug.Log($"Damage dealt to {playerEnemy.name}: {damageToShield}");
                }
            }

        }
        // PASO 3: Notificación. RETORNAR información sobre el daño aplicado
        /*
         * ?. Verifica si hay métodos suscritos antes de ejecutar 
         * .Invoke(): Ejecuta TODOS los métodos suscritos al evento
         * Pasa como parámetros si se aplicó daño exitosamente (damageDealt) y el total de daño aplicado (totalDamageDealt) a cada método suscritoº
        */
        OnDamageDealt?.Invoke(damageDealt, totalDamageDealt);
    }

    
}
