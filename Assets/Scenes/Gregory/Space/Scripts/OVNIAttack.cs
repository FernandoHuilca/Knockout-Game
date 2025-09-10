using UnityEngine;

public class OVNIAttack : MonoBehaviour
{
    [SerializeField] private float damage = 15;
    [SerializeField] private float damageToShield = 10;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        Damageable damageable = collider.gameObject.GetComponent<Damageable>();
        Shieldable shieldable = collider.gameObject.GetComponent<Shieldable>();

        if (damageable == null || shieldable == null)
        {
            return;
        }

        if (shieldable == null || !shieldable.IsShieldActive())
        {
            damageable.decreaseLife(damage);
            Debug.Log("We performAttack1 " + collider.gameObject.name);
            return;
        }
        shieldable.decreaseShieldCapacity(damageToShield);
    }
}
