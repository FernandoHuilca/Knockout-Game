using System;
using UnityEngine;
using UnityEngine.UI;
public class Player2Health : MonoBehaviour
{
    public Image healthBarFill; // Referencia al Image de la barra de vida (HealthBarFill)
    public float currentHealth;
    public float maxHealth = 100f;

    private void Start()
    {
        currentHealth = maxHealth;
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
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player 2");
        // TODO: play die animation
        Destroy(gameObject);
    }
}
