using UnityEngine;

public class Player2State : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima por vida
    public int currentHealth; // Salud actual
    public int lives = 3; // Número total de vidas

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth; // Inicializar salud al máximo al inicio
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0; // Evita que la salud sea negativa
        }

        if (currentHealth == 0)
        {
            LoseLife(); // Método que reduce una vida si la salud llega a 0
        }
    }

    // Método para perder una vida y resetear la salud
    void LoseLife()
    {
        lives--; // Reduce una vida
        if (lives > 0)
        {
            currentHealth = maxHealth; // Resetea la salud si aún tiene vidas
        }
        else
        {
            Die(); // Llama al método de muerte si no quedan vidas
        }
    }

    // Método para manejar la muerte del jugador
    void Die()
    {
        isDead = true;
        // Aquí puedes poner una animación de muerte si tienes una
        // Desactiva o destruye el objeto del jugador
        Destroy(gameObject); // Destruye el objeto cuando muere
    }
}
