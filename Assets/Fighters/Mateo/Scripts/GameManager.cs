using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [SerializeField] public List<FightersData> fightersData;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip knockoutVoice;
    [SerializeField] private GameObject puaseUI;
    [SerializeField] private KeyCode pauseKey = KeyCode.P;

    [SerializeField] private AudioClip fighterSelectionAudio;

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "FighterSelectionMenu")
        {
            SoundsController.Instance.RunSound(fighterSelectionAudio);
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

    public void enableGameOverPanel(string looserUserTag)
    {
        string winnerUserTag = looserUserTag == "User1" ? "User2" : "User1";
        GameObject winner = GameObject.FindGameObjectWithTag(winnerUserTag);
        //gameOverUI.setSpriteRenderer(winner.GetComponent<SpriteRenderer>());
        Image winnerImage = winner.GetComponent<Image>();
        Transform childTransform = gameOverUI.transform.transform.Find("WinnerImage");
        childTransform.GetComponent<Image>().sprite = winnerImage.sprite;

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

