using UnityEngine;
using UnityEngine.SceneManagement; //Librer�a necesaria para cambiar entre escenas

public class MainMenu : MonoBehaviour // Manera de declarar una clase en C#
{
    // M�todo para comenzar el juego
    public void PlayGame()
    {
        // Carga la siguiente escena (la escena del juego)
        SceneManager.LoadScene("GameScene"); // Aseg�rate de tener una escena llamada "GameScene" para cargar.
    }
}
