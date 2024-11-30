using UnityEngine;

public class Player1Attack : MonoBehaviour
{
    public int attackDamage = 20;
    public KeyCode attackKey = KeyCode.B;
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
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            swordHitbox.gameObject.SetActive(true);
            Debug.Log("swordHitbox activado"); // Agrega esto para verificar si se activa
            animator.SetTrigger("Attack");
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                swordHitbox.gameObject.SetActive(false);
                Debug.Log("swordHitbox desactivado"); // Agrega esto para verificar si se desactiva
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si el ataque golpea a Player2
        if (collision.CompareTag("Player2"))
        {


            Player2State player2 = collision.GetComponent<Player2State>(); // Usa el nombre de la clase correcto

            if (player2 != null)
            {
                player2.TakeDamage(attackDamage);
                Debug.Log("Player2 ha sido golpeado por Player1"); // Imprime en consola cuando golpea a Player2
            }
        }
    }
}
