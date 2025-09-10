using System;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{
    [Header("Health Settings")]
    public float currentHealth;
    public float maxHealth;
    public int livesRemaining;    

    [Header("Sounds")]
    [SerializeField] private AudioClip soundHurt;
    [SerializeField] private AudioClip sounDeath;

    // Components
    private Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    private Rigidbody2D startRigidbody2D;
    private Rigidbody2D rigidBody2D;
    private RigidbodyConstraints2D originalConstraints;

    // Transform
    private Vector2 startPosition;
    //private Vector3 originalLocalScale;
    
    // Scripts
    private Movement movement;
    private Attack attack;
    private Shield shield;
    private SpecialAttack specialAttack;
    private UserConfiguration userConfiguration;
    private UIController UIController;

    private bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();

        currentHealth = maxHealth;

        startPosition = transform.position;
        startRigidbody2D = GetComponent<Rigidbody2D>();
        //originalLocalScale = transform.localScale;

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        shield = GetComponent<Shield>();
        specialAttack = GetComponent<SpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Guarda las restricciones originales del Rigidbody
        originalConstraints = rigidBody2D.constraints;

        isDead = false;
    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    public void decreaseLife(float damage)
    {
        if (isDead)
        {
            return;
        }

        if (currentHealth < 0)
        {
            return;
        }

        currentHealth -= damage;
        SoundsController.Instance.RunSound(soundHurt);
        animator.SetTrigger("hurt");
        //animator.SetBool("isHurt", true);

        if (currentHealth > 0)
        {
            updateUI();
            return;
        }
        manageDead();
    }

    public void manageDead()
    {
        // Desactivar scripts para que no pueda hacer nada 
        specialAttack.enabled = false;
        attack.enabled = false;
        movement.enabled = false;
        shield.enabled = false;

        isDead = true;

        animator.SetTrigger("die"); // Animación de muerte iniciada

        rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        livesRemaining--; 

        SoundsController.Instance.RunSound(sounDeath);

        StartCoroutine(DeathAnimationFallback());
    }

    private System.Collections.IEnumerator DeathAnimationFallback()
    {
        // Esperar um tiempo mayor a la duración de la animación de muerte. Con esto se asegura que primero se visualiza la animación
        // de muerte completa y luego se ejecuta el resto de la lógica de onDeathAnimationComplete() (Respawn, muerte definitiva, etc.)
        yield return new WaitForSeconds(0.90f);

        onDeathAnimationComplete();
    }

    private void respawn()
    {
        // Desactiva la simulación del Rigidbody temporalmente.
        startRigidbody2D.simulated = false;

        // Hace que el jugador sea invisible temporalmente.
        //transform.localScale = Vector3.zero;

        // Restablece la posición inicial del jugador.
        transform.position = startPosition;

        // Restaurar la orientación basada en `facingRight`.
        /*if (userConfiguration == null)
        {
            return;
        }
        userConfiguration.setFacingRight(userConfiguration.getFacingRight());*/


        // Restaura las restricciones originales del Rigidbody.
        //rigidBody2D.constraints = originalConstraints;

        // Hace visible al jugador y reactiva la simulación.
        //transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;

        // Asegúrate de que todos los scripts estén habilitados.
        specialAttack.enabled = true;
        attack.enabled = true;
        movement.enabled = true;
        shield.enabled = true;

        isDead = false;

        Debug.Log("Respawn completed successfully.");
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

    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }

}
