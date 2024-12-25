using System;
using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public int livesRemaining;

    private Vector2 startPosition;
    private Rigidbody2D startRigidbody2D;
    private Vector3 originalLocalScale;
    private FighterMovement fighterMovement;

    private UIController UIController;

    private void Start()
    {
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();

        currentHealth = maxHealth;

        startPosition = transform.position;
        startRigidbody2D = GetComponent<Rigidbody2D>();
        originalLocalScale = transform.localScale;

        fighterMovement = GetComponent<FighterMovement>();
    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    internal void decreaselife(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            livesRemaining--;

            if (livesRemaining == 0)
            {
                die();
            }
            else
            {
                respawn();
                currentHealth = maxHealth;
            }
        }
        updateUI();
    }

    private void respawn()
    {
        startRigidbody2D.simulated = false;

        // Hace que el jugador sea invisible temporalmente (usando scale)
        transform.localScale = Vector3.zero;

        // Restablece la posición inicial del jugador
        transform.position = startPosition;

        // Restaurar la orientación basada en `facingRight`
        if (fighterMovement != null)
        {
            fighterMovement.setFacingRight(fighterMovement.GetFacingRight());
        }
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;
    }


    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        Destroy(gameObject);
    }

    public void setMaxHealth(float healthFromPersonaje)
    {
        maxHealth = healthFromPersonaje;
    }
}
