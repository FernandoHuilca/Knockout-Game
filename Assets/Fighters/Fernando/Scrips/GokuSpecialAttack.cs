using Unity.VisualScripting;
using UnityEngine;

public class GokuSpecialAttack : MonoBehaviour
{
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga máxima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial está listo.
    private UIController UIController; // Referencia al controlador de la UI.

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
            Debug.Log("Special Attack Activated!");
            performSpecialAttack(); // Aquí colocas la lógica del ataque especial.
            specialCharge = 0f; // Reiniciar la barra.
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {

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
