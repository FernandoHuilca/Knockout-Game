using UnityEngine;

public class Genkidama : MonoBehaviour
{
    public Animator animator;
    private float speed;
    private float damage;
    private string userTag; 
    private Transform target; // Referencia al enemigo a seguir.
    private float followTime; // Tiempo durante el cual seguirá al enemigo.
    private float elapsedTime; // Tiempo transcurrido desde que comenzó a seguir.

    public void Initialize(float speed, float damage, string userTag, Transform target, float followTime)
    {
        this.speed = speed;
        this.damage = damage;
        this.userTag = userTag;
        this.target = target;
        this.followTime = followTime;
        this.elapsedTime = 0f; // Reiniciar el tiempo transcurrido
        animator = GetComponent<Animator>();
        animator.SetTrigger("GenkidamaIdle");
    }


    private void Update()
    {
        if (target != null && elapsedTime < followTime)
        {
            // Incrementar el tiempo transcurrido.
            elapsedTime += Time.deltaTime;

            // Calcular la dirección hacia el objetivo.
            Vector3 direction = (target.position - transform.position).normalized;

            // Mover la Genkidama hacia el objetivo.
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            // Si el tiempo ha terminado o el objetivo es nulo, mover en línea recta.
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Evitar colisionar con el usuario que lanzó la Genkidama.
        if (other.tag == userTag)
            return;

        // Aplicar daño al enemigo.
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.decreaseLife(damage);
        }

        // Destruir la Genkidama tras colisionar.
        Destroy(gameObject);
    }
}
