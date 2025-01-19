using UnityEngine;

public class PursuerProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidad del proyectil
    [SerializeField] private float damage = 10f; // Daño que causa el proyectil
    private Vector3 targetPosition;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    private void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            // Mueve el proyectil hacia el objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Destruir el proyectil si alcanza el objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
                Debug.Log("El proyectil alcanzó al objetivo.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si golpeó al jugador
        if (other.CompareTag("User1") || other.CompareTag("User2"))
        {
            Debug.Log($"Impacto al jugador {other.tag}");

            // Aplicar daño al enemigo.
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.decreaseLife(damage);
            }

            // Destruir el proyectil tras impactar
            Destroy(gameObject);
        }
    }
}
