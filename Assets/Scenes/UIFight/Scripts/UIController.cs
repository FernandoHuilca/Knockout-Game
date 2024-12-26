using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Referencias a los elementos visuales de la UI
    [Header("UI Elements")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private List<Image> lives;
    [SerializeField] private Image specialBarFill;

    // Actualiza la barra de salud
    public void updateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        }
    }

    // Actualiza las vidas restantes
    public void updateLives(int livesRemaining)
    {
        for (int i = 0; i < lives.Count; i++)
        {
            lives[i].enabled = i < livesRemaining;
        }
    }

    // Actualiza la barra de carga del ataque especial
    public void updateSpecialBar(float currentCharge, float maxCharge)
    {
        if (specialBarFill != null)
        {
            specialBarFill.fillAmount = Mathf.Clamp(currentCharge / maxCharge, 0, 1);
        }
    }

    // Métodos para asignar referencias desde otros scripts
    public void setHealthBarFill(Image healthBar)
    {
        this.healthBarFill = healthBar;
    }

    public void setLives(List<Image> livesList)
    {
        lives = livesList;
    }

    public int getNumberOfLives()
    {
        return lives.Count;
    }

    public void setSpecialBarFill(Image specialBar)
    {
        this.specialBarFill = specialBar;
    }
}
