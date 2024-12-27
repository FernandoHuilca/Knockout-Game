using UnityEngine;

public class GalacticOctopus : MonoBehaviour
{
    public float verticalSpeed = 2f; // Velocidad inicial hacia abajo
    public float horizontalSpeed = 3f; // Velocidad horizontal
    public float verticalOscillationSpeed = 0.5f; // Velocidad del movimiento vertical oscilatorio
    public float horizontalMin = 2.88f; // Límite mínimo del movimiento horizontal
    public float horizontalMax = 14.39f; // Límite máximo del movimiento horizontal
    public float verticalMin = 26.5f; // Límite mínimo del movimiento vertical oscilatorio
    public float verticalMax = 27.5f; // Límite máximo del movimiento vertical oscilatorio
    public float finalYPosition = 29.29f; // Posición final en Y
    public float oscillationDuration = 11.45f; // Duración de la oscilación en segundos

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
        Debug.Log(other.tag);
        Debug.Log(userTag);
        // Compara si la capa del objeto coincide con la capa deseada
        if (other.gameObject.layer == LayerMask.NameToLayer("BaseFighter") && other.tag != userTag)
        {

            // Asegúrate de que el componente Health existe antes de intentar usarlo
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            //Health healthComponent = other.gameObject.GetComponent<Health>();
            if (damageable!= null)
            {
                damageable.decreaseLife(10);
                //healthComponent.decreaseLife(10);
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
        
        //laser.gameObject.GetComponent<Collider2D>().enabled = state;
        //laser.gameObject.GetComponent<Laser>().enabled = state;
    }

    private void deactiveLaser()
    {
        manipulateLaser("LeftLaser", false);
        manipulateLaser("RightLaser", false);
    }

    public void setTag(string tag)
    {
        Debug.Log(tag);
        userTag = tag;
        Debug.Log(userTag);
    }
}
