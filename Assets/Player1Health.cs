using System;
using UnityEngine;
using UnityEngine.UI;
public class Player1Health : MonoBehaviour
{
    public Image healthBarFill; // Referencia al Image de la barra de vida (HealthBarFill)
    public float currentHealth;
    public float maxHealth = 100f;

    public Image[] lives;
    public int livesRemaining;

    private Vector2 startPos;
    private Rigidbody2D playerRb;

    private void Start()
    {
        livesRemaining = lives.Length;
        currentHealth = maxHealth;
        startPos = transform.position;
        playerRb = gameObject.GetComponent<Rigidbody2D>(); 
    
    }

    void Update()
    {
        healthBarFill.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }

    internal void decreaselife(float damage)
    {
        currentHealth -= damage;

        // TODO:play hurt animation

        if (currentHealth <= 0)
        {
            Update();

            // Decrease the value of livesRemaining
            livesRemaining--;
            // Hide one of the life images
            lives[livesRemaining].enabled = false;

            // If we run out of lives we lose the game
            if (livesRemaining == 0)
            {
                Die();
            }

            respawn();
            currentHealth = 100;

        }
    }

    private void respawn()
    {
        playerRb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);

        
        transform.position = startPos;
        
        transform.localScale = new Vector3(1, 1, 1);
        playerRb.simulated = true;
    }

    private void Die()
    {
        Debug.Log("Player " + (gameObject.layer == 3 ? "2" : "1"));
        // TODO: play die animation
        Destroy(gameObject);
    }
}