using System.Runtime.CompilerServices;
using UnityEngine;

public class FireBallAttack : MonoBehaviour
{ 
    [SerializeField] private float damage = 50;
    [SerializeField] private float damageToShield = 75;
    [SerializeField] private string userTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        Shieldable shieldable = collision.GetComponent<Shieldable>();

        if (damageable != null && !collision.CompareTag(userTag))
        {
            if (shieldable == null || !shieldable.IsShieldActive())
            {
                damageable.decreaseLife(damage);
                Debug.Log("We hit " + collision.name);
                
            }
            else
            {
                shieldable.decreaseShieldCapacity(damageToShield);
            }
        }
    }

    public void setUserTag(string userTag) {
        this.userTag = userTag;
    }
}
