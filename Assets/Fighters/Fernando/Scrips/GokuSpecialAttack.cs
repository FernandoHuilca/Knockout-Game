using Unity.VisualScripting;
using UnityEngine;

public class GokuSpecialAttack : MonoBehaviour
{
    private bool isPerformingSpecialAttack = false; //Bandera para controlar si el personaje está usando el ataque especial
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga máxima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial está listo.
    private UIController UIController; // Referencia al controlador de la UI.

    [SerializeField] private GameObject genkidamaPrefab; // Prefab de la Genkidama.
    [SerializeField] private Transform genkidamaSpawnPoint; // Punto de aparición de la Genkidama.
    [SerializeField] private float genkidamaSpeed = 2f; // Velocidad del movimiento de la Genkidama.
    [SerializeField] private float genkidamaDamage = 75f; // Daño de la Genkidama.
    [SerializeField] private float followTime = 8f; // Tiempo de persecución.
    [SerializeField] private float animationDuration = 10;//Duración de la animación de la genkidama stateInfo.length;
    [Header("Components")]
    [SerializeField] private AudioClip soundSpecialAttack;
    [SerializeField] private AudioSource audioSource;


    private void Start()
    {
        UIController = GetComponent<UIController>();
        animator = GetComponent<Animator>();
        updateUI();

    }

    // Método que aumenta la barra de carga.
    public void increaseCharge(float amount)
    {
        if (!isReady) // Si el ataque especial no está listo, cargar la barra.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Asegurarse de que no pase de 100.

            if (specialCharge >= maxCharge) // Si la barra está llena, marcar como listo.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }


    // Método para usar el ataque especial.
    public void useSpecialAttack()
    {
        if (isReady) // Solo se puede usar si está completamente cargada.
        {
            if (isPerformingSpecialAttack)
                return;
            isPerformingSpecialAttack = true;
            if (audioSource != null && soundSpecialAttack != null)
            {
                audioSource.clip = soundSpecialAttack;
                audioSource.Play();
            }

            animator.SetTrigger("specialAttack");
            // Calcular duración de la animación
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Llamar a lanzar la Genkidama después de la animación.
            Invoke(nameof(performSpecialAttack), animationDuration);
            specialCharge = 0f; // Reiniciar la barra.
            isReady = false;
            
        }
    }
    private void EndSpecialAttack()
    {
        isPerformingSpecialAttack = false;
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        updateUI();
    }

    private void performSpecialAttack()
    {
        Debug.Log("Performing the Genkidama!");

        // Instanciar la Genkidama en el punto de aparición.
        GameObject genkidamaInstance = Instantiate(genkidamaPrefab, genkidamaSpawnPoint.position, Quaternion.identity);

        // Buscar al enemigo más cercano.
        Transform target = FindNearestEnemy();

        // Configurar la Genkidama.
        Genkidama genkidamaScript = genkidamaInstance.GetComponent<Genkidama>();
        if (genkidamaScript != null)
        {
            genkidamaScript.Initialize(genkidamaSpeed, genkidamaDamage, gameObject.tag, target, followTime);
        }

        // Finalizar el ataque especial.
        EndSpecialAttack();
    }


    private Transform FindNearestEnemy()
    {
        float minDistance = float.MaxValue; // Inicializamos con un valor alto.
        Transform nearestEnemy = null; // Inicializamos como null.

        // Encuentra todos los objetos en la capa "BaseFighter".
        GameObject[] potentialEnemies = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in potentialEnemies)
        {
            if (gameObject.tag == "User1") 
            {
                // Excluimos a este objeto y verificamos que el otro tenga el tag "User2".
                if (obj != gameObject && obj.tag == "User2")
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = obj.transform;
                    }
                }
            }
            else 
            {
                if (obj != gameObject && obj.tag == "User1")
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = obj.transform;
                    }
                }
            }
        }

        return nearestEnemy; // Devuelve el enemigo más cercano o null si no hay.
    }





    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
    // Método para leer el estado
    public bool IsPerformingSpecialAttack() => isPerformingSpecialAttack;
}