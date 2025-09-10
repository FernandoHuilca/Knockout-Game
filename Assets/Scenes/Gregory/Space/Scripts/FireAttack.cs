using UnityEngine;

public class FireAttack : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private float damageToShield = 5;

    private void OnTriggerEnter2D(Collider2D collision)
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
            Debug.Log("FIRE: We performAttack1 " + collision.gameObject.name);
            return;
        }
        shieldable.decreaseShieldCapacity(damageToShield);
    }
}
