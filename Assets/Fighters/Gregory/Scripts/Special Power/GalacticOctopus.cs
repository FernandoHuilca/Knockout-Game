using UnityEngine;

public class GalacticOctopus : MonoBehaviour
{
    public float verticalSpeed; // Velocidad inicial hacia abajo
    public float horizontalSpeed; // Velocidad horizontal
    public float verticalOscillationSpeed; // Velocidad del movimiento vertical oscilatorio
    public float horizontalMin; // Límite mínimo del movimiento horizontal
    public float horizontalMax; // Límite máximo del movimiento horizontal
    public float verticalMin; // Límite mínimo del movimiento vertical oscilatorio
    public float verticalMax; // Límite máximo del movimiento vertical oscilatorio
    public float finalYPosition; // Posición final en Y
    public float oscillationDuration; // Duración de la oscilación en segundos

    private float startTime;
    private Vector3 initialPosition;
    private bool isOscillating = false;
    [SerializeField] private string userTag;

    void Start()
    {
        initialPosition = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        if (!isOscillating)
        {
            // Movimiento hacia abajo
            transform.position += Vector3.down * verticalSpeed * Time.deltaTime;

            // Si alcanzó la posición mínima en Y, empieza la oscilación
            if (transform.position.y <= verticalMin)
            {
                isOscillating = true;
                startTime = Time.time; // Reinicia el temporizador
            }
        }
        else
        {
            // Tiempo desde que comenzó la oscilación
            float elapsedTime = Time.time - startTime;

            if (elapsedTime < oscillationDuration)
            {
                // Movimiento oscilatorio horizontal
                float horizontal = Mathf.PingPong(Time.time * horizontalSpeed, horizontalMax - horizontalMin) + horizontalMin;

                // Movimiento oscilatorio vertical
                float vertical = Mathf.PingPong(Time.time * verticalOscillationSpeed, verticalMax - verticalMin) + verticalMin;

                // Aplica la posición oscilatoria
                transform.position = new Vector3(horizontal, vertical, initialPosition.z);
            }
            else
            {
                // Movimiento final hacia arriba
                transform.position += Vector3.up * verticalSpeed * Time.deltaTime;

                // Si alcanza la posición final en Y, detiene el movimiento
                if (transform.position.y >= finalYPosition)
                {
                    Destroy(gameObject); // Destruye el objeto
                    //enabled = false; // Desactiva este script
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Compara si la capa del objeto coincide con la capa deseada
        if (other.gameObject.layer == LayerMask.NameToLayer("BaseFighter") && other.tag != userTag)
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

    private void activeLaser()
    {
        manipulateLaser("LeftLaser", true);
        manipulateLaser("RightLaser", true);
    }

    private void manipulateLaser(string gameObjectLaser, bool state)
    {
        Transform laser = transform.Find(gameObjectLaser);
        laser.GetComponent<Laser>().setTag(userTag);
        laser.gameObject.SetActive(state);
    }

    private void deactiveLaser()
    {
        manipulateLaser("LeftLaser", false);
        manipulateLaser("RightLaser", false);
    }

    public void setTag(string tag)
    {
        userTag = tag;
    }
}
