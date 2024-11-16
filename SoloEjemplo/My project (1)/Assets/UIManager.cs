using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player 1 UI Elements")]
    public Image player1HealthFill; // Referencia al relleno de la barra de salud de Player1
    public Image[] player1Lives; // Array de imágenes para las vidas de Player1
    public Player1State player1; // Referencia al script de estado de Player1

    [Header("Player 2 UI Elements")]
    public Image player2HealthFill; // Referencia al relleno de la barra de salud de Player2
    public Image[] player2Lives; // Array de imágenes para las vidas de Player2
    public Player2State player2; // Referencia al script de estado de Player2

    void Update()
    {
        // Actualiza la barra de salud de Player1
        player1HealthFill.fillAmount = (float)player1.currentHealth / player1.maxHealth;
        Debug.Log("Player1 Health Fill: " + player1HealthFill.fillAmount); // Verifica el valor de fillAmount

        // Actualiza las bolitas de vida de Player1
        for (int i = 0; i < player1Lives.Length; i++)
        {
            player1Lives[i].enabled = i < player1.lives;
        }

        // Actualiza la barra de salud de Player2
        player2HealthFill.fillAmount = (float)player2.currentHealth / player2.maxHealth;
        Debug.Log("Player2 Health Fill: " + player2HealthFill.fillAmount); // Verifica el valor de fillAmount

        // Actualiza las bolitas de vida de Player2
        for (int i = 0; i < player2Lives.Length; i++)
        {
            player2Lives[i].enabled = i < player2.lives;
        }
    }

}
