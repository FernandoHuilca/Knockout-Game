using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Código para destruir al enemigo o desactivarlo
        Destroy(gameObject);
    }
}
