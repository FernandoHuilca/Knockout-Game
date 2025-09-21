using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [SerializeField] public List<FightersData> fightersData;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private GameObject puaseUI;
    [SerializeField] private KeyCode pauseKey = KeyCode.P;

    [SerializeField] private AudioClip fighterSelectionAudio;

    private DamageToEnemies damageToEnemies;

    // Control de pausa
    public static bool isPaused = false;

    // Almacenar estado de componentes para restaurarlos
    private Dictionary<MonoBehaviour, bool> componentStates = new Dictionary<MonoBehaviour, bool>();

    private static bool isEESpaceDiscovered = false;

    public static void setIsEESpaceDiscovered(bool eeFlag)
    {
        isEESpaceDiscovered = eeFlag;
    }

    public static bool getIsEESpaceDiscovered()
    {
        return isEESpaceDiscovered;
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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "FighterSelectionMenu")
        {
            PlayerPrefs.SetString("User1", "");
            PlayerPrefs.Save();
            PlayerPrefs.SetString("User2", "");
            PlayerPrefs.Save();
            SoundsController.Instance.RunSound(fighterSelectionAudio);

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey) && puaseUI != null && !gameOverUI.activeSelf)
        {
            togglePause();
        }
    }

    private void togglePause()
    {
        puaseUI.SetActive(!puaseUI.activeSelf);
        Time.timeScale = puaseUI.activeSelf ? 0 : 1;

        if (puaseUI.activeSelf)
        {
            pauseGame();
        }
        else
        {
            resumeGame();
        }
    }

    private void pauseGame()
    {
        SoundsController.Instance.pauseSound();  // Pausar todos los sonidos

        disableScripts();
    }

    private void resumeGame()
    {
        // Restaurar estados de componentes
        foreach (var kvp in componentStates)
        {
            if (kvp.Key != null)
            {
                kvp.Key.enabled = kvp.Value; // Restaurar estado original
            }
        }

        // Reanudar sonidos
        if (SoundsController.Instance != null)
        {
            SoundsController.Instance.reactiveSound();
        }
    }

    public void disableScripts()
    {
        // Limpiar diccionarios para nueva pausa
        componentStates.Clear();

        // Obtener todos los GameObjects en la escena
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // Saltar objetos de UI de pausa y game over
            if (obj.layer == LayerMask.NameToLayer("UI"))
            {
                continue;
            }

            if (obj.transform.parent != null && obj.GetComponent<Camera>() != null)
            {
                continue;
            }

            if (obj.transform.parent != null && obj.GetComponent<EventSystem>() != null)
            {
                continue;
            }

            if (obj != null && obj.GetComponent<SoundsController>() != null)
            {
                continue;
            }

            if(obj != null && obj.GetComponent<LeanTween>() != null)
            {
                continue;
            }

            if (obj != null && obj.GetComponent<UIController>() != null)
            {
                continue;
            }

            // Obtener todos los componentes del objeto
            MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
            
            foreach (MonoBehaviour component in components)
            {
                // Guardar estado original y deshabilitar
                if (component != null && component != this)
                {
                    Debug.Log(component.name);
                    componentStates[component] = component.enabled;
                    component.enabled = false;
                }
            }
        }
    }

    public void enableGameOverPanel(string looserUserTag)
    {
        disableScripts();

        string winnerUserTag = looserUserTag == "User1" ? "User2" : "User1";
        GameObject winner = GameObject.FindGameObjectWithTag(winnerUserTag);
        //gameOverUI.setSpriteRenderer(winner.GetComponent<SpriteRenderer>());
        Image winnerImage = winner.GetComponent<Image>();
        Transform childTransform = gameOverUI.transform.transform.Find("WinnerImage");
        childTransform.GetComponent<Image>().sprite = winnerImage.sprite;

        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        //SoundsController.Instance.pauseSound();  // Pausar todos los sonidos
        GameObject.Find("SoundsController").GetComponent<AudioSource>().clip = gameOverSound;
        //GameObject.Find("SoundsController").GetComponent<AudioSource>().Play();
        SoundsController.Instance.RunSound(gameOverSound);

        TextMeshProUGUI textMeshProUGUI = gameOverUI.transform.Find("KnockoutTMP").GetComponent<TextMeshProUGUI>();

        if (winnerUserTag == "User1" && !string.IsNullOrEmpty(PlayerPrefs.GetString("User1")))
        {
            string nameWinner = PlayerPrefs.GetString("User1");
            textMeshProUGUI.text = nameWinner + " is the winner!";
            Debug.Log("El ganador es: " + nameWinner);
            updateWinnerScore(nameWinner);
        }
        else if (winnerUserTag == "User2" && !string.IsNullOrEmpty(PlayerPrefs.GetString("User2")))
        {
            string nameWinner = PlayerPrefs.GetString("User2");
            textMeshProUGUI.text = nameWinner + " is the winner!";
            Debug.Log("El ganador es: " + nameWinner);
            updateWinnerScore(nameWinner);
        }
        else if (winnerUserTag == "User1")
        {
            Debug.Log("Valor de User1: " + PlayerPrefs.GetString("User1"));
            textMeshProUGUI.text = "Player 1 is the winner!";
        }
        else
        {
            Debug.Log("Valor de User2: " + PlayerPrefs.GetString("User2"));
            textMeshProUGUI.text = "Player 2 is the winner!";
        }
    }

    public void resume()
    {
        puaseUI.SetActive(false);
        Time.timeScale = 1;
        //SoundsController.Instance.reactiveSound();  // Reanudar todos los sonidos
        resumeGame();
        //SoundsController.Instance.ResumeAllSounds();  // Reanudar todos los sonidos
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
            Debug.Log("Resuming music...");
            MenuMusicManager.Instance.ResumeMusic();
        }
        
        PlayerPrefs.SetString("User1", "");
        PlayerPrefs.Save();
        PlayerPrefs.SetString("User2", "");
        PlayerPrefs.Save();

        resumeGame();

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

        PlayerPrefs.SetString("User1", "");
        PlayerPrefs.Save();
        PlayerPrefs.SetString("User2", "");
        PlayerPrefs.Save();

        resumeGame();

        SceneManager.LoadScene("FighterSelectionMenu");
        Time.timeScale = 1; 
    }

    private void updateWinnerScore(string username)
    {
        string dbName = "URI=file:LeaderboardDB.db"; // Ruta de la base de datos

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Sumar 5 al score del usuario en una sola consulta
                command.CommandText = "UPDATE User SET score = score + 5 WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Debug.Log("Puntaje actualizado para: " + username);
                else
                    Debug.LogError("Usuario no encontrado en la base de datos: " + username);
            }

            connection.Close();
        }
    }

    public void setPauseKey(KeyCode keyCode)
    {
        pauseKey = keyCode;
    }
}

