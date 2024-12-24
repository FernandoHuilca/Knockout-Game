using System;
using UnityEngine;
using UnityEngine.UI;

public class Player1Health : MonoBehaviour
{
    // Referencia al componente de imagen de la barra de vida
    public Image healthBarFill;

    // Salud actual y máxima del jugador
    public float currentHealth;
    public float maxHealth = 100f;

    // Arreglo de imágenes de "vidas" en la interfaz
    public Image[] lives;
    public int livesRemaining;

    // Posición inicial del jugador (para reaparecer)
    private Vector2 startPos;

    // Componente Rigidbody2D del jugador (para física)
    private Rigidbody2D playerRb;

    private void Start()
    {
        // Inicializa las vidas y la salud
        livesRemaining = lives.Length;
        currentHealth = maxHealth;

        // Guarda la posición inicial y obtiene el Rigidbody2D
        startPos = transform.position;
        playerRb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Actualiza la barra de vida visualmente (entre 0 y 1)
        healthBarFill.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;  // Aumenta la salud por la cantidad especificada
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Asegura que la salud no supere el máximo
    }

    // Método para reducir la vida del jugador
    internal void decreaselife(float damage)
    {
        currentHealth -= damage; // Reduce la salud del jugador

        // Si la salud llega a 0
        if (currentHealth <= 0)
        {
            Update(); // Actualiza la barra de vida

            livesRemaining--; // Reduce una vida
            lives[livesRemaining].enabled = false; // Oculta una imagen de vida

            // Si no quedan más vidas, el jugador "muere"
            if (livesRemaining == 0)
            {
                Die();
            }

            // Reaparece al jugador y restablece su salud
            respawn();
            currentHealth = 100;
        }
    }

    // Método para reaparecer al jugador
    private void respawn()
    {
        // Desactiva las físicas temporalmente
        playerRb.simulated = false;

        // Hace invisible al jugador temporalmente
        transform.localScale = new Vector3(0, 0, 0);

        // Mueve al jugador a su posición inicial
        transform.position = startPos;

        // Restaura su tamaño y re-activa las físicas
        transform.localScale = new Vector3(1, 1, 1);
        playerRb.simulated = true;
    }

    // Método para manejar la "muerte" del jugador
    private void Die()
    {
        // Muestra un mensaje indicando qué jugador murió
        Debug.Log("Player " + (gameObject.layer == 3 ? "2" : "1"));

        // TODO: Agregar animación de muerte
        Destroy(gameObject); // Elimina el objeto del jugador de la escena
    }
}
