using UnityEngine;

public class GalacticOctopusLogic : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float verticalSpeed; // Velocidad inicial hacia abajo
    [SerializeField] private float horizontalSpeed; // Velocidad horizontal
    [SerializeField] private float verticalOscillationSpeed; // Velocidad del movimiento vertical oscilatorio

    [Header("Movement Boundaries")]
    [SerializeField] private float horizontalMin; // Límite mínimo del movimiento horizontal
    [SerializeField] private float horizontalMax; // Límite máximo del movimiento horizontal
    [SerializeField] private float verticalMin; // Límite mínimo del movimiento vertical oscilatorio
    [SerializeField] private float verticalMax; // Límite máximo del movimiento vertical oscilatorio
    [SerializeField] private float oscillationDuration; // Duración de la oscilación en segundos
    [SerializeField] private bool isOscillating = false;

    [Header("Position Settings")]
    [SerializeField] private float finalYPosition;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float startTime;

    [Header("Attack")]    
    [SerializeField] private string userTag;
    [SerializeField] private float attackValueToShield;
    [SerializeField] private float attackValue;
    [SerializeField] private GameObject leftLaserPrefab;
    [SerializeField] private GameObject rightLaserPrefab;

    [Header("Sounds")]
    //[SerializeField] private AudioClip soundVoice1;
    [SerializeField] private AudioClip soundVoice2;
    //[SerializeField] private AudioClip chargingSound;
    [SerializeField] private AudioClip laserSound;

    void Start()
    {
        initialPosition = transform.position;
        startTime = Time.time;
        SoundsController.Instance.RunSound(soundVoice2);
    }

    void Update()
    {
        if (!isOscillating)
        {
            // Movimiento hacia abajo
            transform.position += Vector3.down * verticalSpeed * Time.deltaTime;

            if (transform.position.y > verticalMin)
            {
                return;
            }
            // Si alcanzó la posición mínima en Y, empieza la oscilación
            isOscillating = true;
            startTime = Time.time; // Reinicia el temporizador
            return;
        }

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
            return;
        }
        // Movimiento final hacia arriba
        transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
        

        if (transform.position.y < finalYPosition)
        {
            return;
        }
        // Si alcanza la posición final en Y, detiene el movimiento
        Destroy(gameObject); // Destruye el objeto
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Compara si la capa del objeto coincide con la capa deseada
        if (other.gameObject.layer != LayerMask.NameToLayer("BaseFighter") || other.tag == userTag)
        {
            return;
        }

        Damageable damageableOtherFighter = other.gameObject.GetComponent<Damageable>();
        Shieldable shieldableOtherFighter = other.gameObject.GetComponent<Shieldable>();

        if (damageableOtherFighter == null)
        {
            return;
        }

        if (shieldableOtherFighter == null || !shieldableOtherFighter.IsShieldActive())
        {
            damageableOtherFighter.decreaseLife(attackValue);
            Debug.Log("We performAttack1 " + other.gameObject.name);
            return;
        }
        shieldableOtherFighter.decreaseShieldCapacity(attackValueToShield);
    }

    private void prepareLaser()
    {
        //SoundsController.Instance.RunSound(chargingSound);
    }

    private void activeLaser()
    {
        SoundsController.Instance.RunSound(laserSound);
        //SoundsController.Instance.pauseSound();
        //SoundsController.Instance.RunSound(laserSound);
        //SoundsController.Instance.RunSound(laserSound);

        manipulateLaser("LeftLaser", true);
        manipulateLaser("RightLaser", true);
    }

    private void manipulateLaser(string gameObjectLaser, bool state)
    {
        Transform laser = transform.Find(gameObjectLaser);
        laser.GetComponent<Laser>().setTag(userTag);
        laser.gameObject.SetActive(state);
    }

    private void desactiveLaser()
    {
        //SoundsController.Instance.pauseSound();
        manipulateLaser("LeftLaser", false);
        manipulateLaser("RightLaser", false);
    }

    public void setTag(string tag)
    {
        userTag = tag;
    }
}
