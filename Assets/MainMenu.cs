using UnityEngine;
using UnityEngine.SceneManagement; //Librería necesaria para cambiar entre escenas

public class MainMenu : MonoBehaviour // Manera de declarar una clase en C#
{
    // Método para comenzar el juego
    public void PlayGame()
    {
        // Carga la siguiente escena (la escena del juego)
        SceneManager.LoadScene("GameScene"); // Asegúrate de tener una escena llamada "GameScene" para cargar.
    }
}
