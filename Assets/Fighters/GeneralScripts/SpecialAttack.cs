using Unity.VisualScripting;
using UnityEngine;

public abstract class SpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga m�xima para activar el ataque especial.

    private UIController UIController; // Referencia al controlador de la UI.

   public void Start()
    {
        UIController = GetComponent<UIController>();
        updateUI();
        
    }

    // M�todo que aumenta la barra de carga.
    public void increaseCharge(float amount)
    {
        specialCharge += amount;
        if(specialCharge > maxCharge)
        {
            specialCharge = maxCharge;
        }
        updateUI();
    }

    // M�todo para usar el ataque especial.
    public virtual void useSpecialAttack()
    {
        if (specialCharge < maxCharge)
        {
            return;
        }
        Debug.Log("Special Attack Activated!");
        performSpecialAttack(); // Aqu� colocas la l�gica del ataque especial.
        reduceCharge(100);
    }

    public void reduceCharge(float amount)
    {
        specialCharge -= amount;
        if (specialCharge < 0.0f)
        {
            specialCharge = 0.0f;
        }
        updateUI();
    }

    public abstract void performSpecialAttack();

    protected void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
}
