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

    public InputField enterName;

    public InputField enterPassword;

    public string username;

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


    public void registerUser()
    {
        if (enterName.text != string.Empty && enterPassword.text != string.Empty)
        {
            if (isUserExists(enterName.text))
            {
                Debug.Log("El usuario ya existe. Intenta con otro nombre.");
            }
            else
            {
                addUser(enterName.text, enterPassword.text);
                Debug.Log("Usuario registrado exitosamente.");

                enterName.text = string.Empty;
                enterPassword.text = string.Empty;
            }
        }
        else
        {
            Debug.Log("El nombre de usuario y la contraseña no pueden estar vacíos.");
        }
    }

    public void loginUser()
    {
        if (enterName.text != string.Empty && enterPassword.text != string.Empty)
        {
            if (validateUser(enterName.text, enterPassword.text))
            {
                Debug.Log("Inicio de sesión exitoso. ¡Bienvenido!");
                username = enterName.text;
                Debug.Log(username);

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
