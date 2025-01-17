using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonController : MonoBehaviour
{
    // Esta funci�n es llamada cuando se presiona un bot�n espec�fico
    public void LoadSceneByButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
