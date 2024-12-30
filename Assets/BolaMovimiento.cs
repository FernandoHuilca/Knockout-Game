using UnityEngine;

public class BolaMovimiento : MonoBehaviour
{
    private Vector2 direcci�n; // Direcci�n en la que la bola se expandir�
    private float velocidadExpansi�n;
    private string userTag;

    public void Inicializar(Vector2 dir, float velocidad)
    {
        direcci�n = dir.normalized; // Asegura que la direcci�n est� normalizada
        velocidadExpansi�n = velocidad;
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
        // Mueve la bola en la direcci�n calculada
        transform.position += (Vector3)direcci�n * velocidadExpansi�n * Time.deltaTime;

        // Destruye la bola si est� fuera de los l�mites de la pantalla
        if (Est�FueraDePantalla())
        {
            Destroy(gameObject);
        }
    }

    private bool Est�FueraDePantalla()
    {
        Vector3 posicionEnPantalla = Camera.main.WorldToViewportPoint(transform.position);

        // Comprueba si la bola est� fuera del rango visible
        return posicionEnPantalla.x < 0 || posicionEnPantalla.x > 1 ||
               posicionEnPantalla.y < 0 || posicionEnPantalla.y > 1;
    }
}