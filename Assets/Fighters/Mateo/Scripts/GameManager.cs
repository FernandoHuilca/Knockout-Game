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

    private void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

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
        //if (gameOverUI.activeInHierarchy) { 
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //    return;
        //}
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void enableGameOverPanel(string userTag)
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        SoundsController.Instance.pauseSound();
        SoundsController.Instance.RunSound(knockoutVoice);
    }

    public void restartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;

    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void goToFighterSelectionMenu()
    {
        SceneManager.LoadScene("FighterSelectionMenu");
        Time.timeScale = 1;
    }

}

