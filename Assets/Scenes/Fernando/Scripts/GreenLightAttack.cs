using UnityEngine;

public class GreenLightAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioClip soundRedSound; // Sonido que se reproduce antes del ataque
    [SerializeField] private AudioSource audioSource; // Fuente de audio para reproducir el sonido

    [SerializeField] private float initialDelay = 8f; // Tiempo de espera inicial en segundos
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField] private Transform spawnPoint; // Punto de aparición del proyectil
    [SerializeField] private float attackIntervalMin = 8f; // Intervalo mínimo entre ataques
    [SerializeField] private float attackIntervalMax = 15f; // Intervalo máximo entre ataques
    [SerializeField] private float activeDuration = 5f; // Duración activa del ataque
    [SerializeField] private float fireCooldown = 1f; // Tiempo mínimo entre disparos

    [Header("Backgrounds")]
    [SerializeField] private SpriteRenderer backgroundRenderer; // El SpriteRenderer del fondo
    [SerializeField] private Sprite attackBackground; // El fondo durante el ataque
    [SerializeField] private Sprite defaultBackground; // El fondo por defecto

    private bool isAttackActive = false; // Bandera para determinar si el ataque está activo
    private Vector3 lastPlayer1Position; // Última posición conocida del jugador 1
    private Vector3 lastPlayer2Position; // Última posición conocida del jugador 2
    private GameObject player1;
    private GameObject player2;
    private float lastFireTime = 0f; // Tiempo del último disparo

    private void Start()
    {
        // Encuentra los jugadores en el escenario
        player1 = GameObject.FindGameObjectWithTag("User1");
        player2 = GameObject.FindGameObjectWithTag("User2");

        if (player1 != null) lastPlayer1Position = player1.transform.position;
        else Debug.LogWarning("Player1 (User1) no encontrado en la escena.");

        if (player2 != null) lastPlayer2Position = player2.transform.position;
        else Debug.LogWarning("Player2 (User2) no encontrado en la escena.");

        // Retrasar el inicio del ciclo de ataque
        Invoke(nameof(StartAttackCycle), initialDelay);
    }

    private void Update()
    {
        if (isAttackActive)
        {
            // Monitorea el movimiento de los jugadores
            CheckPlayerMovement(player1, ref lastPlayer1Position);
            CheckPlayerMovement(player2, ref lastPlayer2Position);
        }
    }

    private void StartAttackCycle()
    {
        ScheduleNextAttack();
    }

    private void ScheduleNextAttack()
    {
        float randomInterval = Random.Range(attackIntervalMin, attackIntervalMax);
        Debug.Log($"Siguiente ataque programado en {randomInterval} segundos.");
        Invoke(nameof(PrepareForAttack), randomInterval);
    }

    private void PrepareForAttack()
    {
        // Reproduce el sonido de advertencia 5 segundos antes de activar el ataque
        PlayWarningSound();
        Invoke(nameof(ToggleAttack), 5f); // Activa el ataque después de 5 segundos
    }

    private void PlayWarningSound()
    {
        if (audioSource != null && soundRedSound != null)
        {
            audioSource.PlayOneShot(soundRedSound);
            Debug.Log("Sonido de advertencia reproducido.");
        }
        else
        {
            Debug.LogWarning("AudioSource o AudioClip no están configurados correctamente.");
        }
    }

    private void ToggleAttack()
    {
        UpdatePlayerPositions();
        isAttackActive = true;
        Debug.Log("¡Ataque activado! No te muevas.");

        // Cambia el fondo al de ataque
        if (backgroundRenderer != null && attackBackground != null)
        {
            backgroundRenderer.sprite = attackBackground;
        }

        Invoke(nameof(EndAttack), activeDuration); // Termina el ataque después de la duración activa
    }

    private void EndAttack()
    {
        isAttackActive = false;
        Debug.Log("El ataque ha terminado.");

        // Vuelve al fondo original
        if (backgroundRenderer != null && defaultBackground != null)
        {
            backgroundRenderer.sprite = defaultBackground;
        }

        ScheduleNextAttack(); // Programa el próximo ataque al terminar el actual
    }

    private void CheckPlayerMovement(GameObject player, ref Vector3 lastPosition)
    {
        if (player == null) return;

        // Detecta si el jugador se movió
        if (Vector3.Distance(player.transform.position, lastPosition) > 0.1f) // Precisión de movimiento
        {
            if (Time.time >= lastFireTime + fireCooldown) // Respetar el tiempo de espera entre disparos
            {
                Debug.Log($"{player.tag} se movió mientras el ataque estaba activo.");
                FireProjectile(player.transform.position);
                lastPosition = player.transform.position; // Actualiza la posición
                lastFireTime = Time.time; // Actualiza el tiempo del último disparo
            }
        }
    }

    private void FireProjectile(Vector3 targetPosition)
    {
        // Instancia el proyectil en el punto de spawn
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        // Configura el proyectil para que persiga al jugador que se movió
        PursuerProjectile pursuer = projectile.GetComponent<PursuerProjectile>();
        if (pursuer != null)
        {
            pursuer.SetTarget(targetPosition);
        }
        else
        {
            Debug.LogWarning("PursuerProjectile no encontrado en el proyectil.");
        }
    }
    private void UpdatePlayerPositions()
    {
        // Actualiza las posiciones de los jugadores justo antes de comenzar el ataque
        if (player1 != null) lastPlayer1Position = player1.transform.position;
        if (player2 != null) lastPlayer2Position = player2.transform.position;

        Debug.Log("Posiciones de los jugadores actualizadas al final del sonido de advertencia.");
    }
}
