using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject InstructionsUI;
    [SerializeField] private GameObject CreditsUI;


    public void startGame()
    {
        SceneManager.LoadScene("FighterSelectionMenu");
    }

    public void loadInstructionsScene() {
        InstructionsUI.SetActive(true);
    }

    public void loadCreditsScene()
    {
        CreditsUI.SetActive(true);
    }

    public void back()
    {
        if(InstructionsUI.activeSelf)
        {
            InstructionsUI.SetActive(false);
            return;
        }
        CreditsUI.SetActive(false);

    }
}
