using UnityEngine;
using UnityEngine.UI;

public class Player1State : MonoBehaviour
{
    public float maxHealth = 100f; // Salud m�xima
    public float currentHealth; // Salud actual
    public int lives = 3; // N�mero total de vidas
    private bool isDead = false;

    [SerializeField] private Image HealthBarImage;

    void Start()
    {
        currentHealth = maxHealth; // Inicializa la salud al m�ximo al inicio
        UpdateHealthBar(); // Actualiza la barra de vida al inicio
    }

    private void Update()
    {
        // Aseg�rate de mantener la salud en el rango permitido
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // M�todo para recibir da�o (se invoca externamente)
    public void TakeDamage(int damage)
    {
        StartCoroutine(GradualHealthReduction(damage)); // Llama a la corutina
    }

    // Corutina para reducir la salud progresivamente
    private System.Collections.IEnumerator GradualHealthReduction(int damage)
    {
        float targetHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth); // Calcula la nueva salud objetivo
        while (currentHealth > targetHealth)
        {
            currentHealth -= 1f; // Reduce la salud gradualmente
            UpdateHealthBar(); // Actualiza la barra visualmente
            yield return new WaitForSeconds(0.01f); // Espera un peque�o intervalo de tiempo
        }

        // Aseg�rate de establecer la salud exacta al final
        currentHealth = targetHealth;

        // Si la salud llega a 0, pierde una vida
        if (currentHealth == 0)
        {
            LoseLife();
        }
    }

    // Actualiza la barra de vida en la interfaz
    private void UpdateHealthBar()
    {
        if (HealthBarImage != null)
        {
            HealthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    // M�todo para perder una vida
    void LoseLife()
    {
        lives--;
        if (lives > 0)
        {
            currentHealth = maxHealth; // Resetea la salud si a�n hay vidas
        }
        else
        {
            Die(); // Si no quedan vidas, ejecuta el m�todo de muerte
        }
    }

    // M�todo para manejar la muerte del jugador
    void Die()
    {
        isDead = true;
        Destroy(gameObject); // Destruye el objeto del jugador
    }
}
