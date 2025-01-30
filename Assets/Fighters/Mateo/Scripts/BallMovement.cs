using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 direction; // Dirección en la que la bola se expandirá
    private float expansionSpeed;
    private string userTag;

    public float specialPowerDamage;
    public float specialPowerDamageToShield;

    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir.normalized; // Asegura que la dirección esté normalizada
        expansionSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(userTag))
        {
            Debug.Log(userTag);
            Debug.Log(other.gameObject.tag);
            return;
        }

        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        Shieldable shield = other.gameObject.GetComponent<Shieldable>();

        if (damageable != null)
        {
            if (shield == null || !shield.IsShieldActive())
            {
                damageable.decreaseLife(specialPowerDamage);
            }
            else
            {
                shield.decreaseShieldCapacity(specialPowerDamageToShield);
            }
        }
    }

    public void setUserTag(string userTag)
    {
        this.userTag = userTag;
    }

    void Update()
    {
        // Mueve la bola en la dirección calculada
        transform.position += (Vector3)direction * expansionSpeed * Time.deltaTime;

        // Destruye la bola si está fuera de los límites de la pantalla
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfScreen()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Comprueba si la bola está fuera del rango visible
        return screenPosition.x < 0 || screenPosition.x > 1 ||
               screenPosition.y < 0 || screenPosition.y > 1;
    }
}