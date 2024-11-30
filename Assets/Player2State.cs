using UnityEngine;

public class Player2State : MonoBehaviour
{
    public int maxHealth = 100; // Salud m�xima por vida
    public int currentHealth; // Salud actual
    public int lives = 3; // N�mero total de vidas

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth; // Inicializar salud al m�ximo al inicio
    }

    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0; // Evita que la salud sea negativa
        }

        if (currentHealth == 0)
        {
            LoseLife(); // M�todo que reduce una vida si la salud llega a 0
        }
    }

    // M�todo para perder una vida y resetear la salud
    void LoseLife()
    {
        lives--; // Reduce una vida
        if (lives > 0)
        {
            currentHealth = maxHealth; // Resetea la salud si a�n tiene vidas
        }
        else
        {
            Die(); // Llama al m�todo de muerte si no quedan vidas
        }
    }

    // M�todo para manejar la muerte del jugador
    void Die()
    {
        isDead = true;
        // Aqu� puedes poner una animaci�n de muerte si tienes una
        // Desactiva o destruye el objeto del jugador
        Destroy(gameObject); // Destruye el objeto cuando muere
    }
}
