using UnityEngine;
using UnityEngine.UI;

public class Player1State : MonoBehaviour
{
    public float maxHealth = 100f; // Salud máxima
    public float currentHealth; // Salud actual
    public int lives = 3; // Número total de vidas
    private bool isDead = false;

    [SerializeField] private Image HealthBarImage;

    void Start()
    {
        currentHealth = maxHealth; // Inicializa la salud al máximo al inicio
        UpdateHealthBar(); // Actualiza la barra de vida al inicio
    }

    private void Update()
    {
        // Asegúrate de mantener la salud en el rango permitido
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // Método para recibir daño (se invoca externamente)
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
            yield return new WaitForSeconds(0.01f); // Espera un pequeño intervalo de tiempo
        }

        // Asegúrate de establecer la salud exacta al final
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

    // Método para perder una vida
    void LoseLife()
    {
        lives--;
        if (lives > 0)
        {
            currentHealth = maxHealth; // Resetea la salud si aún hay vidas
        }
        else
        {
            Die(); // Si no quedan vidas, ejecuta el método de muerte
        }
    }

    // Método para manejar la muerte del jugador
    void Die()
    {
        isDead = true;
        Destroy(gameObject); // Destruye el objeto del jugador
    }
}
