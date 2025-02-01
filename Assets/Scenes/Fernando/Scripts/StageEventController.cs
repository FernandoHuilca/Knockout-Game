using UnityEngine;

public class StageEventController : MonoBehaviour
{
    public GameObject shenlongPrefab; // Prefab de Shenlong

    [Header("Límites del Escenario")]
    public Vector2 spawnPositionMin; // Límite inferior para la posición inicial de Shenlong
    public Vector2 spawnPositionMax; // Límite superior para la posición inicial de Shenlong
    public Vector2 targetPositionMin; // Límite inferior para la posición final de Shenlong
    public Vector2 targetPositionMax; // Límite superior para la posición final de Shenlong

    [Header("Intervalo de Aparición")]
    public float minSpawnInterval = 10f; // Tiempo mínimo entre apariciones
    public float maxSpawnInterval = 20f; // Tiempo máximo entre apariciones

    private float nextSpawnTime; // Tiempo restante para la próxima aparición

    void Start()
    {
        // Calcular el tiempo inicial para la primera aparición
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        nextSpawnTime -= Time.deltaTime;

        if (nextSpawnTime <= 0f)
        {
            SpawnShenlong();
            // Calcular el tiempo aleatorio para la siguiente aparición
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnShenlong()
    {
        // Crear una posición inicial y final aleatoria dentro de los límites
        Vector2 startPosition = new Vector2(
            Random.Range(spawnPositionMin.x, spawnPositionMax.x),
            Random.Range(spawnPositionMin.y, spawnPositionMax.y)
        );

        Vector2 endPosition = new Vector2(
            Random.Range(targetPositionMin.x, targetPositionMax.x),
            Random.Range(targetPositionMin.y, targetPositionMax.y)
        );

        // Instanciar a Shenlong
        GameObject shenlong = Instantiate(shenlongPrefab);
        ShenlongController controller = shenlong.GetComponent<ShenlongController>();

        if (controller != null)
        {
            controller.startPosition = startPosition;
            controller.endPosition = endPosition;
        }

        Debug.Log("¡Shenlong ha aparecido!");
    }
}
