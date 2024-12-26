using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void buttonStart()
    {
        SceneManager.LoadScene("FighterSelectionMenu");
    }
}
