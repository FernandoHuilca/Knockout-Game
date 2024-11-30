using UnityEngine;

public class Player2Attack : MonoBehaviour
{
    public int attackDamage = 20;
    public KeyCode attackKey = KeyCode.F;
    public Transform swordHitbox;
    public float attackDuration = 0.2f;

    private Animator animator;
    private bool isAttacking = false;
    private float attackTimer;

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtener el Animator del jugador
        swordHitbox.gameObject.SetActive(false); // Asegurarse de que el área de impacto esté inactiva al inicio
    }

    void Update()
    {
        // Inicia el ataque cuando se presiona la tecla de ataque
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            swordHitbox.gameObject.SetActive(true); // Activa el área de impacto
            animator.SetTrigger("Attack"); // Activa el trigger para la animación de ataque
        }

        // Desactiva el área de impacto después de la duración del ataque
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                swordHitbox.gameObject.SetActive(false); // Desactiva el área de impacto
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si el ataque golpea a Player1
        if (collision.CompareTag("Player1"))
        {
            // Player1State player1 = collision.GetComponent<Player1State>(); // Usa el nombre de la clase correcto
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.currentHealth -= 20f;
                playerHealth.decreaselife(-20f);
                //player1.TakeDamage(attackDamage);
                Debug.Log("Player1 ha sido golpeado por Player2"); // Imprime en consola cuando golpea a Player1
            }
        }
    }
}
