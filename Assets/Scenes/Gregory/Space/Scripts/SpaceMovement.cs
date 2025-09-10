using UnityEngine;

public class SpaceMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float verticalFrequency;
    [SerializeField] private bool movingToEnd = true;
    [SerializeField] private float timeToDestroy = 11;
    [SerializeField] private float distanceToCreateCopy = 20;

    void Start()
    {
        transform.position = startPosition;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 target = endPosition; // Determinar el objetivo actual

        // Movimiento horizontal hacia el objetivo
        Vector2 currentPosition = transform.position;
        Vector2 direction = (target - currentPosition).normalized;
        float distance = Vector2.Distance(currentPosition, target);

        // Movimiento lineal en el eje horizontal
        Vector2 horizontalMove = direction * horizontalSpeed * Time.deltaTime;

        // Movimiento oscilatorio en el eje vertical
        float verticalOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;

        // Aplicar movimiento combinado
        transform.position = new Vector2(currentPosition.x + horizontalMove.x, startPosition.y + verticalOffset);

        if (distance > distanceToCreateCopy || !movingToEnd) // Si está cerca del objetivo
        {
            return;
        }
        CreateCopyAndDestroyOriginal();
        movingToEnd = false;
        Destroy(gameObject, timeToDestroy); // Destruir el objeto original después de 11 segundos
    }

    void CreateCopyAndDestroyOriginal()
    {
        // Crear copia del GameObject y del Script
        GameObject copy = Instantiate(gameObject);
        SpaceMovement copyScript = copy.GetComponent<SpaceMovement>();

        // Configurar la copia
        copyScript.startPosition = startPosition;
        copyScript.endPosition = endPosition;
    }

}
