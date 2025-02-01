using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework.Internal.Filters;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    private string dbName = "URI=file:LeaderboardDB.db";

    private List<Leaderboard> leaderboards = new List<Leaderboard>();

    public GameObject scorePrefab;

    public Transform scoreParent;

    public int topRanks;

    void Start()
    {
        createDB();

        //addUser("gregorio", "gre");

        // deleteUser(2);

        //getScores();

        showScores();
    }

    private void Update()
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

    private void getScores()
    {
        leaderboards.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(dbName))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM User";

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Debug.Log(reader.GetString(1) + " " + reader.GetInt32(3));

                        leaderboards.Add(new Leaderboard(reader.GetInt32(0), reader.GetString(1),
                                                         reader.GetString(2), reader.GetInt32(3)));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        leaderboards.Sort();
    }

    private void deleteUser (int userID)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM User " +
                                      "WHERE userID = " + userID + ";";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    private void showScores()
    {
        getScores();

        foreach (GameObject score in GameObject.FindGameObjectsWithTag("score"))
        {
            Destroy(score);
        }

        for (int i = 0; i < topRanks; i++)
        {
            if (i <= leaderboards.Count - 1)
            {
                GameObject tmpObject = Instantiate(scorePrefab);

                Leaderboard tmpScore = leaderboards[i];

                tmpObject.GetComponent<LeaderboardScript>().setScore(tmpScore.username, tmpScore.score.ToString(), "#" + (i + 1).ToString());

                tmpObject.transform.SetParent(scoreParent);

                tmpObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // Esta función es llamada cuando se presiona un botón específico
    public void LoadSceneByButton(string sceneName)
    {
        
        /*// Detener la música si es una escena de pelea
        if (sceneName != "FighterSelectionMenu")
        {
            if (MenuMusicManager.Instance != null)
                MenuMusicManager.Instance.StopMusic();
        }*/

        SceneManager.LoadScene(sceneName);
    }
}
