using System;
using UnityEngine;
using UnityEngine.UI;
public class Player1Health : MonoBehaviour
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
        Debug.Log("Player " + (gameObject.layer == 3 ? "2" : "1"));
        // TODO: play die animation
        Destroy(gameObject);
    }
}
