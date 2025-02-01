using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework.Internal.Filters;
using System.Xml.Linq;

public class UserDBManager : MonoBehaviour
{
    private string dbName = "URI=file:LeaderboardDB.db";

    public InputField enterNameUser1;
    public InputField enterPasswordUser1;

    public InputField enterNameUser2;
    public InputField enterPasswordUser2;

    public string username;
    [SerializeField] private string userTag;

    public Text advertisementUser1;
    public Text advertisementUser2;

    void Start()
    {
        username = null;
        ShowMessage(advertisementUser1, "Welcome, Player 1", new Color(0.95f, 0.52f, 0.0f));
        ShowMessage(advertisementUser2, "Welcome, Player 2", new Color(0.95f, 0.52f, 0.0f));
        createDB();
    }

    void Update()
    {

    }

    public void createDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS User (userID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                                                                        "username TEXT NOT NULL UNIQUE, " +
                                                                        "password TEXT NOT NULL, " +
                                                                        "score INTEGER);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void addUser(string username, string password)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO User (username, password, score) " +
                                      "VALUES ('" + username + "', '" + password + "', 0);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
    public void registerUser(InputField enterNameUser, InputField enterPasswordUser, Text advertisement)
    {
        string username = enterNameUser.text;
        string password = enterPasswordUser.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            if (isUserExists(username))
            {
                ShowMessage(advertisement, "User exists", Color.red);
            }
            else
            {
                addUser(username, password);
                ShowMessage(advertisement, "Registered", Color.green);
                ClearInputFields(enterNameUser, enterPasswordUser);
            }
        }
        else
        {
            ShowMessage(advertisement, "Fields empty", Color.red);
        }
    }

    public void registerUser1() => registerUser(enterNameUser1, enterPasswordUser1, advertisementUser1);
    public void registerUser2() => registerUser(enterNameUser2, enterPasswordUser2, advertisementUser2);

    public void loginUser(InputField enterNameUser, InputField enterPasswordUser, string userTag, Text advertisement)
    {
        string enteredUsername = enterNameUser.text.Trim(); // Elimina espacios en blanco
        string user1 = PlayerPrefs.GetString("User1", "");
        string user2 = PlayerPrefs.GetString("User2", "");

        // Verificar si los campos están vacíos primero
        if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enterPasswordUser.text))
        {
            ShowMessage(advertisement, "Fields empty", Color.red);
            return;
        }

        // Evitar que detecte un usuario vacío como "en uso"
        if (((userTag == "User1" && enteredUsername == user2) ||
             (userTag == "User2" && enteredUsername == user1)))
        {
            ShowMessage(advertisement, "User in use", Color.red);
            return;
        }

        // Validar usuario y contraseña en la base de datos
        if (validateUser(enteredUsername, enterPasswordUser.text))
        {
            PlayerPrefs.SetString(userTag, enteredUsername);
            PlayerPrefs.Save();
            ShowMessage(advertisement, "Welcome, " + enteredUsername, Color.green);
            ClearInputFields(enterNameUser, enterPasswordUser);
        }
        else
        {
            ShowMessage(advertisement, "Wrong user/pass", Color.red);
        }
    }


    public void loginUser1() => loginUser(enterNameUser1, enterPasswordUser1, "User1", advertisementUser1);
    public void loginUser2() => loginUser(enterNameUser2, enterPasswordUser2, "User2", advertisementUser2);

    private bool isUserExists(string username)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM User WHERE username = @username;";
                command.Parameters.AddWithValue("@username", username);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }

    private bool validateUser(string username, string password)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM User WHERE username = @username AND password = @password;";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }

    private void ShowMessage(Text advertisement, string message, Color color)
    {
        advertisement.text = message;
        advertisement.color = color;
    }

    private void ClearInputFields(InputField nameField, InputField passwordField)
    {
        nameField.text = "";
        passwordField.text = "";
    }
}