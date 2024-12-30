using UnityEngine;

public class BolaMovimiento : MonoBehaviour
{
    private Vector2 dirección; // Dirección en la que la bola se expandirá
    private float velocidadExpansión;
    private string userTag;

    public void Inicializar(Vector2 dir, float velocidad)
    {
        dirección = dir.normalized; // Asegura que la dirección esté normalizada
        velocidadExpansión = velocidad;
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
                damageable.decreaseLife(50);
            }
            else
            {
                shield.decreaseShieldCapacity(30);
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
        transform.position += (Vector3)dirección * velocidadExpansión * Time.deltaTime;

        // Destruye la bola si está fuera de los límites de la pantalla
        if (EstáFueraDePantalla())
        {
            Destroy(gameObject);
        }
    }

    private bool EstáFueraDePantalla()
    {
        Vector3 posicionEnPantalla = Camera.main.WorldToViewportPoint(transform.position);

        // Comprueba si la bola está fuera del rango visible
        return posicionEnPantalla.x < 0 || posicionEnPantalla.x > 1 ||
               posicionEnPantalla.y < 0 || posicionEnPantalla.y > 1;
    }
}