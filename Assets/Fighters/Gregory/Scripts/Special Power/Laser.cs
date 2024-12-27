using UnityEngine;

public class Laser : MonoBehaviour
{
    
    [SerializeField] private string userTag;
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        // Compara si la capa del objeto coincide con la capa deseada
        if (other.gameObject.layer == LayerMask.NameToLayer("BaseFighter") && userTag != other.tag)
        {
            Debug.Log(other.tag);
            Debug.Log(userTag);
            // Asegúrate de que el componente Health existe antes de intentar usarlo
            //Health healthComponent = other.GetComponent<Health>();
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.decreaseLife(20);
                //healthComponent.decreaseLife(2);
            }
        }
    }

    public void setTag(string tag)
    {
        Debug.Log(tag);
        this.userTag = tag;

        Debug.Log(userTag);
    }

}
