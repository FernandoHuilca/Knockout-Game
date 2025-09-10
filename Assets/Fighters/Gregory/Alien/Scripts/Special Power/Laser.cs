using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private string userTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Compara si la capa del objeto coincide con la capa deseada
        if (other.gameObject.layer == LayerMask.NameToLayer("BaseFighter") && userTag != other.tag)
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            Shieldable shield = other.gameObject.GetComponent<Shieldable>();

            if (damageable != null)
            {
                if (shield == null || !shield.IsShieldActive())
                {
                    damageable.decreaseLife(10);
                    Debug.Log("We performAttack1 " + other.gameObject.name);
                }
                else
                {
                    shield.decreaseShieldCapacity(10);
                }
            }
        }
    }

    public void setTag(string tag)
    {
        this.userTag = tag;
    }
}
