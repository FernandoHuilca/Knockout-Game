using UnityEngine;

public class ColliderObstacleAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageToShield;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        Shieldable shieldable = collision.gameObject.GetComponent<Shieldable>();

        if (damageable == null || shieldable == null)
        {
            return;
        }

        if (shieldable == null || !shieldable.IsShieldActive())
        {
            damageable.decreaseLife(damage);
            Debug.Log("Obstacle: We performAttack1 " + collision.gameObject.name);
            return;
        }
        shieldable.decreaseShieldCapacity(damageToShield);
    }
}
