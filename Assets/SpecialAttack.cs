using UnityEngine;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{
    public Image specialBarFill; // Barra visual de carga del ataque especial.
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge = 100f; // Carga máxima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial está listo.

    // Método que aumenta la barra de carga.
    public void IncreaseCharge(float amount)
    {
        if (!isReady) // Si el ataque especial no está listo, cargar la barra.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Asegurarse de que no pase de 100.
            specialBarFill.fillAmount = specialCharge / maxCharge;

            if (specialCharge >= maxCharge) // Si la barra está llena, marcar como listo.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }
        }
    }

    // Método para usar el ataque especial.
    public void UseSpecialAttack()
    {
        if (isReady) // Solo se puede usar si está completamente cargada.
        {
            Debug.Log("Special Attack Activated!");
            PerformSpecialAttack(); // Aquí colocas la lógica del ataque especial.
            specialCharge = 0f; // Reiniciar la barra.
            specialBarFill.fillAmount = 0f;
            isReady = false;
        }
    }

    private void PerformSpecialAttack()
    {
        // TODO: Lógica del ataque especial (daño en área, efectos, animaciones, etc.).
        Debug.Log("Performing the special attack!");
    }
}
