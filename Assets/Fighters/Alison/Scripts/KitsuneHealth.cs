using System;
using UnityEngine;

public class KitsuneHealth : MonoBehaviour, Damageable
{
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public float currentHealth;
    public float maxHealth;
    public int livesRemaining;

    private Vector2 startPosition;
    private Rigidbody2D startRigidbody2D;
    private Vector3 originalLocalScale;
    private KitsuneMovement movement;
    private KitsuneAttack attack;
    private KitsuneShield shield;
    private KitsuneSpecialAttack specialAttack;
    private Rigidbody2D rigidBody2D;
    private RigidbodyConstraints2D originalConstraints;
    private UserConfiguration userConfiguration;
    private UIController UIController;



    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();

        currentHealth = maxHealth;

        startPosition = transform.position;
        startRigidbody2D = GetComponent<Rigidbody2D>();
        originalLocalScale = transform.localScale;

        movement = GetComponent<KitsuneMovement>();
        attack = GetComponent<KitsuneAttack>();
        shield = GetComponent<KitsuneShield>();
        specialAttack = GetComponent<KitsuneSpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Guarda las restricciones originales del Rigidbody
        originalConstraints = rigidBody2D.constraints;
    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    public void decreaseLife(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            specialAttack.enabled = false;
            attack.enabled = false;
            movement.enabled = false;

            livesRemaining--;

            if (livesRemaining > 0)
            {
                currentHealth = maxHealth;
            }

            animator.SetTrigger("die");

        }
        updateUI();
    }

    // Este método se ejecuta al final de la animación de muerte
    public void OnDeathAnimationComplete()
    {


        if (livesRemaining <= 0)
        {
            die();
        }
        else
        {
            currentHealth = maxHealth;
            respawn();
            //currentHealth = maxHealth;
            specialAttack.enabled = true;
            attack.enabled = true;
            movement.enabled = true;
            // Restaura las restricciones originales
            rigidBody2D.constraints = originalConstraints;

            // Corrige ligeramente la posición para forzar el recalculo de colisiones
            rigidBody2D.position = new Vector2(rigidBody2D.position.x, rigidBody2D.position.y + 0.01f);
            animator.SetBool("isDead", false);
        }
    }

    private void respawn()
    {
        // Desactiva la simulación del Rigidbody temporalmente.
        startRigidbody2D.simulated = false;

        // Hace que el jugador sea invisible temporalmente.
        transform.localScale = Vector3.zero;

        // Restablece la posición inicial del jugador.
        transform.position = startPosition;

        // Restaurar la orientación basada en `facingRight`.
        if (userConfiguration != null)
        {
            userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        }

        // Hace visible al jugador y reactiva la simulación.
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;

        Debug.Log("Respawn completed successfully.");
    }



    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }

}
