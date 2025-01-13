using System;
using UnityEngine;

public class AlienHealth : MonoBehaviour, Damageable
{
    [Header("Health Settings")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private int livesRemaining;

    [Header("Respawn Settings")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Rigidbody2D startRigidbody2D;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private RigidbodyConstraints2D originalConstraints;
    [SerializeField] private Vector3 originalLocalScale;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip soundHurt;
    [SerializeField] private AudioClip soundDie;

    [Header("Scripts")]
    [SerializeField] private AlienMovement movement;
    [SerializeField] private AlienAttack attack;
    [SerializeField] private AlienShield shield;
    [SerializeField] private AlienSpecialAttack specialAttack;

    [SerializeField] private UserConfiguration userConfiguration;
    [SerializeField] private UIController UIController;    

    private void Start()
    {
        animator = GetComponent<Animator>();
        startRigidbody2D = GetComponent<Rigidbody2D>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        originalConstraints = rigidBody2D.constraints; // Guarda las restricciones originales del Rigidbody

        startPosition = transform.position;
        originalLocalScale = transform.localScale;

        movement = GetComponent<AlienMovement>();
        attack = GetComponent<AlienAttack>();
        shield = GetComponent<AlienShield>();
        specialAttack = GetComponent<AlienSpecialAttack>();

        userConfiguration = GetComponent<UserConfiguration>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();
        currentHealth = maxHealth;

    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    public void decreaseLife(float damage)
    {
        currentHealth -= damage;
        SoundsController.Instance.RunSound(soundHurt);
        animator.SetTrigger("hurt");

        if (currentHealth > 0)
        {
            updateUI();
            return;
        }
        manageDead();
    }

    public void manageDead()
    {
        rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        specialAttack.enabled = false;
        attack.enabled = false;
        movement.enabled = false;

        livesRemaining--;

        SoundsController.Instance.RunSound(soundDie);
        animator.SetTrigger("die");
    }

    // Este método se ejecuta al final de la animación de muerte
    public void onDeathAnimationComplete()
    {
        if (livesRemaining <= 0)
        {
            die();
        }
        currentHealth = maxHealth;
        respawn();
        
        specialAttack.enabled = true;
        attack.enabled = true;
        movement.enabled = true;
        
        // Restaura las restricciones originales
        rigidBody2D.constraints = originalConstraints;

        // Corrige ligeramente la posición para forzar el recalculo de colisiones
        rigidBody2D.position = new Vector2(rigidBody2D.position.x, rigidBody2D.position.y + 0.01f);
        updateUI();
    }


    private void respawn()
    {
        startRigidbody2D.simulated = false; // Desactiva la simulación del Rigidbody
        transform.localScale = Vector3.zero; // Hace que el jugador sea invisible temporalmente (usando scale)
        transform.position = startPosition; // Restablece la posición inicial del jugador

        // Restaurar la orientación basada en `facingRight`
        if (userConfiguration == null)
        {
            return;
        }
        userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;
    }

    private void die()
    {
        Debug.Log("Game Over");
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }

}
