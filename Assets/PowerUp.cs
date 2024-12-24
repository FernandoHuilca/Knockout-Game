using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float healAmount = 20f; // Cantidad de vida que se restaura al tocar la estrella

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que colisiona tiene el script Player1Health
        Player1Health player1Health = collision.GetComponent<Player1Health>();
        if (player1Health != null)
        {
            if (player1Health.currentHealth < player1Health.maxHealth) // Solo cura si la vida no está completa
            {
                player1Health.IncreaseHealth(healAmount); // Aumenta la vida del jugador 1
                Destroy(gameObject); // Elimina la estrella después de usarse
            }
        }

        // Verifica si el objeto que colisiona tiene el script Player2Health
        Player2Health player2Health = collision.GetComponent<Player2Health>();
        if (player2Health != null)
        {
            if (player2Health.currentHealth < player2Health.maxHealth) // Solo cura si la vida no está completa
            {
                player2Health.currentHealth += healAmount; // Aumenta la vida del jugador 2
                player2Health.currentHealth = Mathf.Clamp(player2Health.currentHealth, 0, player2Health.maxHealth); // Asegura que no supere el máximo
                Destroy(gameObject); // Elimina la estrella después de usarse
            }
        }
    }
}
