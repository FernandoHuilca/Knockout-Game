using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FighterSelectionMenuLogic : MonoBehaviour
{
    // 1. DECLARACIÓN DE ATRIBUTOS

    // Referencia al GameManager
    [Header("GameManager")]
    private GameManager gameManager;

    [Header("User Selection Menu Resources")]
    private int indexUser1; // Indice del personaje seleccionado por el usuario 1
    [SerializeField] private Image user1Image; // Imagen del personaje seleccionado por el usuario 1
    [SerializeField] private TextMeshProUGUI user1Name; // Nombre del personaje seleccionado por el usuario 1

    [Header("User Selection Menu Resources")]
    private int indexUser2; // Indice del personaje seleccionado por el usuario 2
    [SerializeField] private Image user2Image; // Imagen del personaje seleccionado por el usuario 2
    [SerializeField] private TextMeshProUGUI user2Name; // Nombre del personaje seleccionado por el usuario 2


    // ------------------------------------------------------------------------------------------------------------------------------------------
    // 2. MÉTODOS
    private void Start()
    {
        gameManager = GameManager.gameManagerInstance; // Se obtiene la instancia del GameManager

        indexUser1 = PlayerPrefs.GetInt("User1Index"); // Se obtiene el índice del personaje seleccionado por el usuario 1
        indexUser2 = PlayerPrefs.GetInt("User2Index"); // Se obtiene el índice del personaje seleccionado por el usuario 2

        // Se comprueba que los índices no sean mayores que la cantidad de fightersData disponibles
        if (indexUser1 > gameManager.fightersData.Count - 1)
        {
            indexUser1 = 0; // Se asigna el primer personaje si el índice es mayor
        }

        // Se comprueba que los índices no sean mayores que la cantidad de fightersData disponibles
        if (indexUser2 > gameManager.fightersData.Count - 1)
        {
            indexUser2 = 0; // Se asigna el primer personaje si el índice es mayor
        }

        // Se asignan los fightersData seleccionados a los usuarios
        updateUser1SelectionScreen();
        updateUser2SelectionScreen();
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------
    // Actualiza la UI de selección del jugador 1 con la información del personaje seleccionado.
    private void updateUser1SelectionScreen()
    {
        PlayerPrefs.SetInt("User1Index", indexUser1);
        user1Image.sprite = gameManager.fightersData[indexUser1].getFighterImage();
        user1Name.text = gameManager.fightersData[indexUser1].getFighterName();
    }

    // Actualiza la UI de selección del jugador 2 con la información del personaje seleccionado.
    private void updateUser2SelectionScreen()
    {
        PlayerPrefs.SetInt("User2Index", indexUser2);
        user2Image.sprite = gameManager.fightersData[indexUser2].getFighterImage();
        user2Name.text = gameManager.fightersData[indexUser2].getFighterName();
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------
    private int advanceToTheNextFighterUser(int indexUser)
    {
        if (indexUser == gameManager.fightersData.Count - 1)
        {
            indexUser = 0;
        }
        else
        {
            indexUser += 1;
        }
        return indexUser;
    }

    public void advanceToTheNextFighterUser1()
    {
        indexUser1 = advanceToTheNextFighterUser(indexUser1);
        updateUser1SelectionScreen();
    }

    public void advanceToTheNextFighterUser2()
    {
        indexUser2 = advanceToTheNextFighterUser(indexUser2);
        updateUser2SelectionScreen();
    }


    // ------------------------------------------------------------------------------------------------------------------------------------------
    private int goBackToThePreviousFighterUser(int indexUser)
    {
        if (indexUser == 0)
        {
            indexUser = gameManager.fightersData.Count - 1;
        }
        else
        {
            indexUser -= 1;
        }
        return indexUser;
    }

    public void goBackToThePreviousFighterUser1()
    {
        indexUser1 = goBackToThePreviousFighterUser(indexUser1);
        updateUser1SelectionScreen();
    }

    public void goBackToThePreviousFighterUser2()
    {
        indexUser2 = goBackToThePreviousFighterUser(indexUser2);
        updateUser2SelectionScreen();
    }


    // ------------------------------------------------------------------------------------------------------------------------------------------
    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

