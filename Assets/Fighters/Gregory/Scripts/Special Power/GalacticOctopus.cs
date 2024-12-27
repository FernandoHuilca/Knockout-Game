using UnityEngine;

public class GalacticOctopus : MonoBehaviour
{
    public float verticalSpeed = 2f; // Velocidad inicial hacia abajo
    public float horizontalSpeed = 3f; // Velocidad horizontal
    public float verticalOscillationSpeed = 0.5f; // Velocidad del movimiento vertical oscilatorio
    public float horizontalMin = 2.88f; // L�mite m�nimo del movimiento horizontal
    public float horizontalMax = 14.39f; // L�mite m�ximo del movimiento horizontal
    public float verticalMin = 26.5f; // L�mite m�nimo del movimiento vertical oscilatorio
    public float verticalMax = 27.5f; // L�mite m�ximo del movimiento vertical oscilatorio
    public float finalYPosition = 29.29f; // Posici�n final en Y
    public float oscillationDuration = 11.45f; // Duraci�n de la oscilaci�n en segundos

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

            // Si alcanz� la posici�n m�nima en Y, empieza la oscilaci�n
            if (transform.position.y <= verticalMin)
            {
                isOscillating = true;
                startTime = Time.time; // Reinicia el temporizador
            }
        }
        else
        {
            // Tiempo desde que comenz� la oscilaci�n
            float elapsedTime = Time.time - startTime;

            if (elapsedTime < oscillationDuration)
            {
                // Movimiento oscilatorio horizontal
                float horizontal = Mathf.PingPong(Time.time * horizontalSpeed, horizontalMax - horizontalMin) + horizontalMin;

                // Movimiento oscilatorio vertical
                float vertical = Mathf.PingPong(Time.time * verticalOscillationSpeed, verticalMax - verticalMin) + verticalMin;

                // Aplica la posici�n oscilatoria
                transform.position = new Vector3(horizontal, vertical, initialPosition.z);
            }
            else
            {
                // Movimiento final hacia arriba
                transform.position += Vector3.up * verticalSpeed * Time.deltaTime;

                // Si alcanza la posici�n final en Y, detiene el movimiento
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

            // Aseg�rate de que el componente Health existe antes de intentar usarlo
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
