using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [SerializeField] public List<FightersData> fightersData;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip knockoutVoice;
    [SerializeField] private GameObject puaseUI;
    [SerializeField] private KeyCode pauseKey = KeyCode.P;

    private void Awake()
    {
        if (GameManager.gameManagerInstance == null)
        {
            GameManager.gameManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(pauseKey) && puaseUI != null)
        {
            puaseUI.SetActive(!puaseUI.activeSelf);
            Time.timeScale = puaseUI.activeSelf ? 0 : 1;
            SoundsController.Instance.pauseSound();
            //SoundsController.Instance.RunSound(pauseSound);
        }
    }

    public void enableGameOverPanel(string userTag)
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        SoundsController.Instance.pauseSound();
        SoundsController.Instance.RunSound(knockoutVoice);
    }

    public void resume()
    {
        puaseUI.SetActive(false);
        Time.timeScale = 1;
        //SoundsController.Instance.RunSound(pauseSound);
    }

    public void restartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;

    }

    public void goToMainMenu()
    {
        // Al volver al menú principal, asegúrate de que la música se reanude
        if (MenuMusicManager.Instance != null)
        {
            MenuMusicManager.Instance.ResumeMusic();
        }
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void goToFighterSelectionMenu()
    {
        // Al volver al menú principal, asegúrate de que la música se reanude
        if (MenuMusicManager.Instance != null)
        {
            MenuMusicManager.Instance.ResumeMusic();
        }
        SceneManager.LoadScene("FighterSelectionMenu");
        Time.timeScale = 1;
    }

}

