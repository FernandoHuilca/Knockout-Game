using UnityEngine;
using System.Collections;

public class VictorCalderoniSpecialAttack : SpecialAttack
{
    // No ponemos Start() porque sobreescribiría al del padre (No importa si Start() esté vacío)
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private GameObject victorCalderoniCarPrefab;
    [SerializeField] private float durationToFinalPosCar;
    private GameObject carGameObject;
    [SerializeField] private float durationToDecreaseCharge;
    private bool isFalling = false;
    private bool isArrived = false;

    [SerializeField] private Sprite idleSprite;

    void Update()
    {
        if (Input.GetKeyDown(gameObject.GetComponent<UserConfiguration>().getSpecialPowerKey()) && carGameObject != null && isArrived)
        {

            string tag = gameObject.tag;
            Attack attack = gameObject.GetComponent<Attack>();
            float damage = attack.getSpecialPowerDamage();

            carGameObject.transform.Find("Gun").GetComponent<Gun2D>().shoot(damage, damage, tag); // damage y damageToShield deberían ser propias de cada bala
        }
    }

    public override void performSpecialAttack()
    {
        if(carGameObject != null)
        {
            return;
        }

        if (isFalling)
        {
            return;
        }

        Movement movement = gameObject.GetComponent<Movement>();

        if (movement != null && !movement.getIsGrounded()) // o movement.isGrounded si usas la opción 1
        {
            // Si está en el aire, esperar a tocar el suelo
            Debug.Log("Esperando a tocar el suelo para ejecutar el ataque especial...");
            StartCoroutine(waitForGroundedAndExecute(movement));
            return;
        }
        
        prepareSpecialAttackSequence(); // Si está en el suelo, ejecutar inmediatamente
    }

    private IEnumerator waitForGroundedAndExecute(Movement movement)
    {
        isFalling = true;
        // Esperar hasta que toque el suelo
        while (movement != null && !movement.getIsGrounded()) // o !movement.isGrounded
        {
            yield return null; // Esperar un frame
        }
        // Esperar un frame adicional para asegurar que la posición se estabilice
        yield return null;
        // Una vez que toque el suelo, ejecutar la secuencia
        Debug.Log("¡Tocó el suelo! Ejecutando ataque especial...");
        prepareSpecialAttackSequence();
        isFalling = false;
    }

    private void prepareSpecialAttackSequence()
    {
        // 2. Evitar que el luchador se mueva, ataque, use el escudo, reciba daño
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        manageComponents(false);

        Vector2 targetPosition = transform.Find("CarPoint").gameObject.transform.position;
        Vector3 startPosition = new Vector3(targetPosition.x - 50.0f, targetPosition.y, 0.0f);
        carGameObject = Instantiate(victorCalderoniCarPrefab, startPosition, gameObject.transform.rotation);

        Debug.Log("Tag del gameobject que se va a guardar en cargameobject: " + gameObject.tag);
        carGameObject.GetComponent<VictorCalderoniCar>().setTag(gameObject.tag);
        carGameObject.GetComponent<Collider2D>().isTrigger = true;

        StartCoroutine(CompleteAttackSequence(carGameObject, targetPosition));
    }

    private IEnumerator CompleteAttackSequence(GameObject carGameObject, Vector2 targetPosition)
    {
        RigidbodyConstraints2D originalConstraints = gameObject.GetComponent<Rigidbody2D>().constraints;

        Vector3 targetPosDuringSpecialAttack = new Vector3(targetPosition.x, targetPosition.y, 0);
        Vector3 targetPosAfterSpecialAttack = new Vector3(targetPosition.x + 50.0f, targetPosition.y, 0);

        SoundsController.Instance.RunSound(carGameObject.GetComponent<VictorCalderoniCar>().getCarDriftingSound());
        yield return StartCoroutine(moveObject(carGameObject, targetPosDuringSpecialAttack, durationToFinalPosCar)); // 1. Mover a la primera posición

        isArrived = true;

        carGameObject.GetComponent<Collider2D>().isTrigger = false;

        Vector3 posVictorDuringCar = new Vector3(targetPosition.x, targetPosition.y + 1.5f, 0);
        yield return StartCoroutine(moveObject(gameObject, posVictorDuringCar, durationToFinalPosCar / 2.0f));

        yield return new WaitForSeconds(1f); // Espera 1 segundo

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        yield return StartCoroutine(reduceAmount(durationToDecreaseCharge)); // 3. Reducir la barra de carga

        manageComponents(true); // 3. Volver a habilitar componentes
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Animator>().enabled = true;

        gameObject.GetComponent<Rigidbody2D>().constraints = originalConstraints;

        carGameObject.GetComponent<Collider2D>().isTrigger = true;

        SoundsController.Instance.RunSound(carGameObject.GetComponent<VictorCalderoniCar>().getCarDrivindSound());
        yield return StartCoroutine(moveObject(carGameObject, targetPosAfterSpecialAttack, durationToFinalPosCar)); // 5. Mover al carro a la segunda posición

        Destroy(carGameObject);
    }

    public void manageComponents(bool enabled)
    {
        //gameObject.GetComponent<SpriteRenderer>().enabled = enabled;
        //gameObject.GetComponent<CapsuleCollider2D>().enabled = enabled;
        gameObject.GetComponent<Movement>().enabled = enabled;
        //gameObject.GetComponent<Health>().enabled = enabled;
        gameObject.GetComponent<Shield>().enabled = enabled;
        gameObject.GetComponent<VictorCalderoniAttack>().enabled = enabled;

        
    }

    public override void useSpecialAttack()
    {
        if (specialCharge < maxCharge)
        {
            return;
        }
        Debug.Log("Special Attack Activated!");
        performSpecialAttack(); // Aquí colocas la lógica del ataque especial.
    }

    private IEnumerator reduceAmount(float durationToDecrease)
    {
        float startCharge = specialCharge;
        float elapsedTime = 0.0f;

        while (elapsedTime < durationToDecrease)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / durationToDecrease;

            float currentCharge = Mathf.Lerp(startCharge, 0f, t);
            specialCharge = currentCharge;
            updateUI();

            yield return null;
        }
        specialCharge = 0f;
        updateUI();

    }

    private IEnumerator moveObject(GameObject gameObject, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = gameObject.transform.position;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {

            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;

            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;

        }

        gameObject.transform.position = targetPosition;

    }

    public GameObject FindUntaggedObjectWithLayer(int layer)
    {
        // Buscar entre todos los objetos activos
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "Untagged" && obj.layer == layer && obj.name != "GroundCheck")
            {
                return obj;
            }
        }

        return null;
    }
}
