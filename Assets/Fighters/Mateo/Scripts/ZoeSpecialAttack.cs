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
    public GameObject bolaPrefab; // Prefab de la bola
    public int numeroBolas = 20; // Número de bolas en el círculo
    public float velocidadExpansión = 5f; // Velocidad de expansión del círculo
    public int ráfagas = 3; // Número de ráfagas
    public float tiempoEntreRáfagas = 0.5f; // Tiempo entre ráfagas

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


        StartCoroutine(GenerarRáfagas()); // Llama al método para generar las bolas.
    }

    private void special()
    {
        // Activa la animación de ataque
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    private IEnumerator GenerarRáfagas()
    {
        //animator.SetTrigger("balls");
        for (int i = 0; i < ráfagas; i++)
        {
            GenerarCírculo();
            yield return new WaitForSeconds(tiempoEntreRáfagas);
        }
    }

    private void GenerarCírculo()
    {
        float ánguloIncremento = 360f / numeroBolas;

        for (int i = 0; i < numeroBolas; i++)
        {
            // Calcula el ángulo de cada bola
            float ángulo = i * ánguloIncremento * Mathf.Deg2Rad;

            // Genera la posición inicial en el centro del personaje
            Vector2 posicionInicial = transform.position;

            // Crea la dirección radial basada en el ángulo
            Vector2 direcciónRadial = new Vector2(Mathf.Cos(ángulo), Mathf.Sin(ángulo));


            // Instancia la bola en el centro del personaje
            GameObject bola = Instantiate(bolaPrefab, posicionInicial, Quaternion.identity);


            // Inicializa la bola para que se mueva radialmente hacia afuera
            bola.GetComponent<BolaMovimiento>().setUserTag(gameObject.tag);
            bola.GetComponent<BolaMovimiento>().Inicializar(direcciónRadial, velocidadExpansión);
        }
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
}