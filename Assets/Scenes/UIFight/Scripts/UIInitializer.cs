using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIInitializer : MonoBehaviour
{
    private UIController UIController;

    private void Start()
    {
        UIController = GetComponent<UIController>();
    }

    public void configureUI(UIController UIController, int playerIndex, string uiPrefix)
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        Transform healthBarTransform = canvas.transform.Find($"{uiPrefix}/Player{playerIndex}HealthBar");
        if (healthBarTransform != null)
        {
            Image healthBar = healthBarTransform.GetComponent<Image>();
            UIController.setHealthBarFill(healthBar);
        }

        List<Image> lives = new List<Image>();

        for (int i = 0; i < 3; i++)
        {
            Transform lifeTransform = canvas.transform.Find($"{uiPrefix}/Player{playerIndex}NumberLifes/Life{i + 1}");
            if (lifeTransform != null)
            {
                lives.Add(lifeTransform.GetComponent<Image>());
            }
        }
        UIController.setLives(lives);

        Transform specialBarTransform = canvas.transform.Find($"{uiPrefix}/Player{playerIndex}AttackBar");
        if (specialBarTransform != null)
        {
            Image specialBarFill = specialBarTransform.GetComponent<Image>();
            UIController.setSpecialBarFill(specialBarFill);
        }

        // User2NameTMP
        Transform userName = canvas.transform.Find($"{uiPrefix}/User{playerIndex}NameTMP");
        if (userName != null)
        {
            TextMeshProUGUI textMeshPro = userName.GetComponent<TextMeshProUGUI>();
            string playerName = PlayerPrefs.GetString($"User{playerIndex}", ""); // Obtener nombre o vacío

            if (!string.IsNullOrEmpty(playerName)) // Si hay un nombre guardado en PlayerPrefs
            {
                textMeshPro.text = playerName;
            }
            else // Si no hay nombre, asigna "PlayerX"
            {
                textMeshPro.text = "Player " + playerIndex;
            }
        }
    }

}
