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

    void Start()
    {
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


    public void registerUser1()
    {
        //if (enterNameUser1.text != string.Empty && enterPasswordUser1.text != string.Empty)
        //{
        //    if (isUserExists(enterNameUser1.text))
        //    {
        //        Debug.Log("El usuario ya existe. Intenta con otro nombre.");
        //    }
        //    else
        //    {
        //        addUser(enterNameUser1.text, enterPasswordUser1.text);
        //        Debug.Log("Usuario registrado exitosamente.");

        //        enterNameUser1.text = string.Empty;
        //        enterPasswordUser1.text = string.Empty;
        //    }
        //}
        //else
        //{
        //    Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        //}
        registerUser(enterNameUser1, enterPasswordUser1);
    }

    public void registerUser(InputField enterNameUser, InputField enterPasswordUser)
    {
        if (enterNameUser.text != string.Empty && enterPasswordUser.text != string.Empty)
        {
            if (isUserExists(enterNameUser.text))
            {
                Debug.Log("El usuario ya existe. Intenta con otro nombre.");
            }
            else
            {
                addUser(enterNameUser.text, enterPasswordUser.text);
                Debug.Log("Usuario registrado exitosamente.");

                enterNameUser.text = string.Empty;
                enterPasswordUser.text = string.Empty;
            }
        }
        else
        {
            Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        }
    }

    public void registerUser2()
    {
        //if (enterNameUser2.text != string.Empty && enterPasswordUser2.text != string.Empty)
        //{
        //    if (isUserExists(enterNameUser2.text))
        //    {
        //        Debug.Log("El usuario ya existe. Intenta con otro nombre.");
        //    }
        //    else
        //    {
        //        addUser(enterNameUser2.text, enterPasswordUser2.text);
        //        Debug.Log("Usuario registrado exitosamente.");

        //        enterNameUser2.text = string.Empty;
        //        enterPasswordUser2.text = string.Empty;
        //    }
        //}
        //else
        //{
        //    Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        //}
        registerUser(enterNameUser2, enterPasswordUser2);
    }

    public void loginUser1()
    {
        //if (enterNameUser1.tag == "User1")
        //{
        //    Debug.Log("User1");
        //    userTag = "User1";
        //}
        //else
        //{
        //    Debug.Log("User2");
        //    userTag = "User2";
        //}

        //if (enterNameUser1.text != string.Empty && enterPasswordUser1.text != string.Empty)
        //{
        //    if (validateUser(enterNameUser1.text, enterPasswordUser1.text))
        //    {
        //        Debug.Log("Inicio de sesión exitoso. ¡Bienvenido!");
        //        username = enterNameUser1.text;
        //        Debug.Log(username);
        //        PlayerPrefs.SetString(userTag, username);
        //        Debug.Log(PlayerPrefs.GetString(userTag));
        //        Debug.Log(userTag);

        //    }
        //    else
        //    {
        //        Debug.Log("Usuario o contraseña incorrectos.");
        //    }
        //}
        //else
        //{
        //    Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        //}
        loginUser(enterNameUser1, enterPasswordUser1, "User1");
    }

    public void loginUser(InputField enterNameUser, InputField enterPasswordUser, string userTag)
    {

        if (enterNameUser.text != string.Empty && enterPasswordUser.text != string.Empty)
        {
            if (validateUser(enterNameUser.text, enterPasswordUser.text))
            {
                Debug.Log("Inicio de sesión exitoso. ¡Bienvenido!");
                username = enterNameUser.text;
                Debug.Log(username);
                PlayerPrefs.SetString(userTag, username);
                Debug.Log(PlayerPrefs.GetString(userTag));
                Debug.Log(userTag);

            }
            else
            {
                Debug.Log("Usuario o contraseña incorrectos.");
            }
        }
        else
        {
            Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        }
    }

    public void loginUser2()
    {
        //if (enterNameUser2.tag == "User1")
        //{
        //    Debug.Log("User1");
        //    userTag = "User1";
        //}
        //else
        //{
        //    Debug.Log("User2");
        //    userTag = "User2";
        //}

        //if (enterNameUser2.text != string.Empty && enterPasswordUser2.text != string.Empty)
        //{
        //    if (validateUser(enterNameUser2.text, enterPasswordUser2.text))
        //    {
        //        Debug.Log("Inicio de sesión exitoso. ¡Bienvenido!");
        //        username = enterNameUser2.text;
        //        Debug.Log(username);
        //        PlayerPrefs.SetString(userTag, username);
        //        Debug.Log(PlayerPrefs.GetString(userTag));
        //        Debug.Log(userTag);

        //    }
        //    else
        //    {
        //        Debug.Log("Usuario o contraseña incorrectos.");
        //    }
        //}
        //else
        //{
        //    Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        //}
        loginUser(enterNameUser2, enterPasswordUser2, "User2");
    }

    // Verificar si el usuario ya existe en la base de datos
    private bool isUserExists(string username)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM User WHERE username = @username;";
                command.Parameters.AddWithValue("@username", username);

                int userCount = Convert.ToInt32(command.ExecuteScalar());
                return userCount > 0;
            }
        }
    }

    // Validar credenciales del usuario
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

                int userCount = Convert.ToInt32(command.ExecuteScalar());
                return userCount > 0;
            }
        }
    }
}