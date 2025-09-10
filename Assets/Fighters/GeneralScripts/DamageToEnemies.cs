using UnityEngine;

public class DamageToEnemies : MonoBehaviour
{

    public static DamageToEnemies instance { get; private set; }

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

    // M�todo que aplica da�o a los enemigos detectados
    public void applyDamageToEnemies(float damage, float damageToShield, Vector2 position, float attackRange, string tag)
    {
        // Detecta jugadores enemigos dentro del �rea del "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCapsuleAll(weaponHitBox.position, attackRange, )
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(position, attackRange);
        bool damageDealt = false;
        float totalDamageDealt = 0f;

        // Aplica da�o a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            // No atacarse a s� mismo
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
        // RETORNAR informaci�n sobre el da�o aplicado
        OnDamageDealt?.Invoke(damageDealt, totalDamageDealt);
    }

    // Evento para notificar cuando se aplica da�o exitosamente
    public System.Action<bool, float> OnDamageDealt;
}
