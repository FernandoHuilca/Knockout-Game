using System.Collections;
using UnityEngine;

public class ZoeSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga máxima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial está listo.
    private UIController UIController; // Referencia al controlador de la UI.

    public float specialPowerDamage;
    public float specialPowerDamageToShield;
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    private ZoeAttack attack;

    // Configuración para el poder especial con las bolas
    public GameObject ballPrefab; // Prefab de la bola
    public int ballCount = 20; // Número de bolas en el círculo
    public float expansionSpeed = 5f; // Velocidad de expansión del círculo
    public int bursts = 3; // Número de ráfagas
    public float timeBetweenBursts = 0.5f; // Tiempo entre ráfagas

    [SerializeField] private AudioClip soundSpecialAttack1;

    private void Start()
    {
        UIController = GetComponent<UIController>();
        attack = GetComponent<ZoeAttack>(); // Inicializar attack.
        updateUI();
    }

    // Método que aumenta la barra de carga.
    public void increaseCharge(float amount)
    {
        if (!isReady) // Si el ataque especial no está listo, cargar la barra.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Asegurarse de que no pase de 100.

            if (specialCharge >= maxCharge) // Si la barra está llena, marcar como listo.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    // Método para usar el ataque especial.
    public void useSpecialAttack()
    {
        if (isReady) // Solo se puede usar si está completamente cargada.
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack(); // Aquí colocas la lógica del ataque especial.
            specialCharge = 0f; // Reiniciar la barra.
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
        special();
        Debug.Log("Performing the special attack!");
        // La ráfaga será activada por un Animation Event al final de la animación especial.
    }

    private void special()
    {
        // Activa la animación de ataque
        SoundsController.Instance.RunSound(soundSpecialAttack1);
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    // Este método se llamará al finalizar la animación "special" usando un Animation Event.
    private void OnSpecialAnimationEnd()
    {
        StartCoroutine(GenerateBursts());
    }

    private IEnumerator GenerateBursts()
    {
        animator.SetTrigger("balls");
        for (int i = 0; i < bursts; i++)
        {
            GenerateCircle();
            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    private void GenerateCircle()
    {
        float angleIncrement = 360f / ballCount;

        for (int i = 0; i < ballCount; i++)
        {
            // Calcula el ángulo de cada bola
            float angle = i * angleIncrement * Mathf.Deg2Rad;

            // Genera la posición inicial en el centro del personaje
            Vector2 initialPosition = transform.position;

            // Crea la dirección radial basada en el ángulo
            Vector2 radialDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Instancia la bola en el centro del personaje
            GameObject ball = Instantiate(ballPrefab, initialPosition, Quaternion.identity);

            // Inicializa la bola para que se mueva radialmente hacia afuera
            ball.GetComponent<BallMovement>().setUserTag(gameObject.tag);
            ball.GetComponent<BallMovement>().Initialize(radialDirection, expansionSpeed);
        }
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromCharacter)
    {
        this.maxCharge = maxChargeFromCharacter;
    }
}
