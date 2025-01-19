using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonController : MonoBehaviour
{
    // Esta función es llamada cuando se presiona un botón específico
    public void LoadSceneByButton(string sceneName)
    {
        // Detener la música si es una escena de pelea
        if (sceneName != "FighterSelectionMenu")
        {
            if (MenuMusicManager.Instance != null)
                MenuMusicManager.Instance.StopMusic();
        }

        SceneManager.LoadScene(sceneName);
    }
  
}
