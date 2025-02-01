using System;
using System.Collections;
using UnityEngine;

public class ZoeHealth : MonoBehaviour, Damageable
{
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public float currentHealth;
    public float maxHealth;
    public int livesRemaining;

    private Vector2 startPosition;
    private Rigidbody2D startRigidbody2D;
    private Vector3 originalLocalScale;
    private ZoeMovement movement;
    private ZoeAttack attack;
    private ZoeShield shield;
    private ZoeSpecialAttack specialAttack;
    private Rigidbody2D rigidBody2D;
    private RigidbodyConstraints2D originalConstraints;
    private UserConfiguration userConfiguration;
    private UIController UIController;

    [SerializeField] private AudioClip soundHurt;

    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();

        currentHealth = maxHealth;

        startPosition = transform.position;
        startRigidbody2D = GetComponent<Rigidbody2D>();
        originalLocalScale = transform.localScale;

        movement = GetComponent<ZoeMovement>();
        attack = GetComponent<ZoeAttack>();
        shield = GetComponent<ZoeShield>();
        specialAttack = GetComponent<ZoeSpecialAttack>();
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
        SoundsController.Instance.RunSound(soundHurt);
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
            //animator.SetBool("isDead", true);
            animator.SetTrigger("die");

            StartCoroutine(WaitForDeathAnimation());

            //if (livesRemaining == 0)
            //{
            //    die();
            //}
            //else
            //{
            //    respawn();

            //}
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

        startRigidbody2D.simulated = false;

        // Hace que el jugador sea invisible temporalmente (usando scale)
        transform.localScale = Vector3.zero;

        // Restablece la posición inicial del jugador
        transform.position = startPosition;

        // Restaurar la orientación basada en `facingRight`
        if (userConfiguration != null)
        {
            userConfiguration.setFacingRight(userConfiguration.getFacingRight());
            //AlienMovement.setFacingRight(AlienMovement.GetFacingRight());
        }
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;
    }

    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Espera hasta que la animación de muerte esté activa
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null; // Espera un frame
        }

        // Obtén la duración de la animación actual
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length - 0.3f;

        // Espera la duración de la animación
        yield return new WaitForSeconds(animationDuration);

        // Llama a OnDeathAnimationComplete después de la animación
        OnDeathAnimationComplete();
    }
    //public void setMaxHealth(float healthFromPersonaje)
    //{
    //    maxHealth = healthFromPersonaje;
    //}
}