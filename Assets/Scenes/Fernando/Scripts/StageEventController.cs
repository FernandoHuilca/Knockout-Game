using UnityEngine;

public class StageEventController : MonoBehaviour
{
    public GameObject shenlongPrefab; // Prefab de Shenlong
    public Vector2 shenlongStartPosition; // Posici�n inicial de Shenlong
    public Vector2 shenlongEndPosition; // Posici�n final de Shenlong
    public float spawnInterval = 20f; // Intervalo entre apariciones
    public float spawnIntervalRandomness = 5f; // Variaci�n aleatoria del intervalo

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval + Random.Range(-spawnIntervalRandomness, spawnIntervalRandomness))
        {
            SpawnShenlong();
            timer = 0f;
        }
    }

    void SpawnShenlong()
    {
        GameObject shenlong = Instantiate(shenlongPrefab);
        ShenlongController controller = shenlong.GetComponent<ShenlongController>();

        if (controller != null)
        {
            controller.startPosition = shenlongStartPosition;
            controller.endPosition = shenlongEndPosition;
        }

        Debug.Log("�Shenlong ha aparecido!");
    }
}
