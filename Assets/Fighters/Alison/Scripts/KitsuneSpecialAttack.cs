using Unity.VisualScripting;
using UnityEngine;

public class KitsuneSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f;
    public float maxCharge;

    private bool isReady = false;
    private UIController UIController;

    // Agregar una referencia al Animator
    public Animator animator;

    // Prefab para el efecto visual del ataque especial
    public GameObject specialAttackEffect;

    [SerializeField] private GameObject FireBall;

    private void Start()
    {
        UIController = GetComponent<UIController>();
        updateUI();

        // Asegúrate de asignar el Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void increaseCharge(float amount)
    {
        if (!isReady)
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge);

            if (specialCharge >= maxCharge)
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    public void useSpecialAttack()
    {
        if (isReady)
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack();
            specialCharge = 0f;
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
        // Activar animación del ataque especial
        if (animator != null)
        {
            animator.SetTrigger("specialAttack");
        }

        //Vector3 startPosition = new Vector3(transform.position.x + 4, );

        FireBall.GetComponent<FireBallAttack>().setUserTag(gameObject.tag);

        FireBall.GetComponent<FireBallMovement>().setUserConfiguration(gameObject.GetComponent<UserConfiguration>());

        Instantiate(FireBall, transform.position, Quaternion.identity);
        /*
        // Crear el efecto especial si existe el prefab
        if (specialAttackEffect != null)
        {
            Instantiate(specialAttackEffect, transform.position, Quaternion.identity);
        }
        */

        Debug.Log("Performing the special attack!");
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
}
