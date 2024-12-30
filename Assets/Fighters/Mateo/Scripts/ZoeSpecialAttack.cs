using System.Collections;
using UnityEngine;

public class ZoeSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga m�xima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial est� listo.
    private UIController UIController; // Referencia al controlador de la UI.

    public float specialPowerDamage;
    public float specialPowerDamageToShield;
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    private ZoeAttack attack;

    // Configuraci�n para el poder especial con las bolas
    public GameObject bolaPrefab; // Prefab de la bola
    public int numeroBolas = 20; // N�mero de bolas en el c�rculo
    public float velocidadExpansi�n = 5f; // Velocidad de expansi�n del c�rculo
    public int r�fagas = 3; // N�mero de r�fagas
    public float tiempoEntreR�fagas = 0.5f; // Tiempo entre r�fagas

    private void Start()
    {
        UIController = GetComponent<UIController>();
        attack = GetComponent<ZoeAttack>(); // Inicializar attack.
        updateUI();
    }

    // M�todo que aumenta la barra de carga.
    public void increaseCharge(float amount)
    {
        if (!isReady) // Si el ataque especial no est� listo, cargar la barra.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Asegurarse de que no pase de 100.

            if (specialCharge >= maxCharge) // Si la barra est� llena, marcar como listo.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    // M�todo para usar el ataque especial.
    public void useSpecialAttack()
    {
        if (isReady) // Solo se puede usar si est� completamente cargada.
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack(); // Aqu� colocas la l�gica del ataque especial.
            specialCharge = 0f; // Reiniciar la barra.
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
        special();
        Debug.Log("Performing the special attack!");


        StartCoroutine(GenerarR�fagas()); // Llama al m�todo para generar las bolas.
    }

    private void special()
    {
        // Activa la animaci�n de ataque
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    private IEnumerator GenerarR�fagas()
    {
        //animator.SetTrigger("balls");
        for (int i = 0; i < r�fagas; i++)
        {
            GenerarC�rculo();
            yield return new WaitForSeconds(tiempoEntreR�fagas);
        }
    }

    private void GenerarC�rculo()
    {
        float �nguloIncremento = 360f / numeroBolas;

        for (int i = 0; i < numeroBolas; i++)
        {
            // Calcula el �ngulo de cada bola
            float �ngulo = i * �nguloIncremento * Mathf.Deg2Rad;

            // Genera la posici�n inicial en el centro del personaje
            Vector2 posicionInicial = transform.position;

            // Crea la direcci�n radial basada en el �ngulo
            Vector2 direcci�nRadial = new Vector2(Mathf.Cos(�ngulo), Mathf.Sin(�ngulo));


            // Instancia la bola en el centro del personaje
            GameObject bola = Instantiate(bolaPrefab, posicionInicial, Quaternion.identity);


            // Inicializa la bola para que se mueva radialmente hacia afuera
            bola.GetComponent<BolaMovimiento>().setUserTag(gameObject.tag);
            bola.GetComponent<BolaMovimiento>().Inicializar(direcci�nRadial, velocidadExpansi�n);
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