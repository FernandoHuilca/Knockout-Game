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
    
    private bool isDead;

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

        isDead = false;
    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    public void decreaseLife(float damage)
    {
        if (isDead) {
            return;
        }

        if (currentHealth < 0)
        {
            return;
        }

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
        isDead = true;

        rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        specialAttack.enabled = false;
        attack.enabled = false;
        movement.enabled = false;
        shield.enabled = false;

        livesRemaining--;

        SoundsController.Instance.RunSound(soundDie);
        animator.SetTrigger("die");

        StartCoroutine(DeathAnimationFallback());
    }

    private System.Collections.IEnumerator DeathAnimationFallback()
    {
        // Esperar um tiempo mayor a la duración de la animación de muerte. Con esto se asegura que primero se visualiza la animación
        // de muerte completa y luego se ejecuta el resto de la lógica de onDeathAnimationComplete() (Respawn, muerte definitiva, etc.)
        yield return new WaitForSeconds(0.60f);

        onDeathAnimationComplete();
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

        // Asegúrate de que todos los scripts estén habilitados.
        specialAttack.enabled = true;
        attack.enabled = true;
        movement.enabled = true;
        shield.enabled = true;

        isDead = false;
    }

    private void die()
    {
        Debug.Log("Game Over");
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }

}
