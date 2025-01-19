using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonController : MonoBehaviour
{
    // Esta función es llamada cuando se presiona un botón específico
    public void LoadSceneByButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
