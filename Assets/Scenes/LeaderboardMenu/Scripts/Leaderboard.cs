using System;
using UnityEngine;

public class Leaderboard : IComparable<Leaderboard>
{
    public int score {  get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public int userID { get; set; }

    public Leaderboard(int id, string username, string password, int score) 
    { 
        this.score = score;
        this.username = username;
        this.password = password;
        this.userID = id;
    }

    public int CompareTo(Leaderboard other)
    {
        if (other.score < this.score)
        {
            return -1;
        }
        else if (other.score > this.score)
        {
            return 1;
        }

        return 0;
    }
}
