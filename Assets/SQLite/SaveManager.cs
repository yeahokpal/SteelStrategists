/*
 * Programmer: Jack
 * Purpose: Create and Use a Database for saving and loading data
 * Input: Player saves variables to database
 * Output: Loading saved variables
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SaveManager : MonoBehaviour
{
    string dbName = "URI=file:Database.db";
    private void Awake()
    {
        if (!Directory.Exists(Application.streamingAssetsPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves/");
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        if (!File.Exists("Database.db"))
        {
            CreateDB();
        }

        List<string> scores = ReadScores();
    }
    public void CreateDB()
    {
        // Create DB connection
        using (var Connection = new SqliteConnection(dbName))
        {
            Connection.Open();

            // Set up an object command to control database
            IDbCommand Command = Connection.CreateCommand();

            void SendCommand(string cmd)
            {
                Command.CommandText = cmd;
                Command.ExecuteNonQuery();
            }
            
            // Creating the Scores Table
            SendCommand("CREATE TABLE IF NOT EXISTS Scores (PlayerInitials TEXT, Score INTEGER);");

            // Initial Writes to Tables
            for (int i = 0; i < 10; i++)
            {
                SendCommand("INSERT OR REPLACE INTO Scores ('PlayerInitials', 'Score') VALUES ('   ', 0);");
            }

            Connection.Close();
        }
    }

    // Return string list of all scores in the database
    public List<string> ReadScores()
    {
        using (SqliteConnection Connection = new SqliteConnection(dbName))
        {
            Connection.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM Scores", Connection);
            SqliteDataReader reader = cmd.ExecuteReader();

            // Making a string that we return instead so that we can close the Connection
            List<string> scores = new List<string>();
            
            cmd = new SqliteCommand("SELECT * FROM Scores", Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                scores.Add(reader.GetValue(1).ToString() + "," + reader.GetValue(0).ToString());
                print(reader.GetValue(1).ToString() + "," + reader.GetValue(0).ToString());
            }

            Connection.Close();

            return scores;
        }
    }

    // Add a new score and initials to the database
    public void Write(string PlayerInitials, int Score)
    {
        using (var Connection = new SqliteConnection(dbName))
        {
            Connection.Open();

            //Setting up an object command to allow db control
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Scores VALUES ('" + PlayerInitials + "', '" + Score.ToString() + "')";
                command.ExecuteNonQuery();
            }
            Connection.Close();
        }
    }

    public void DeleteDatabase()
    {
        File.Delete("Database.db");
        CreateDB();
    }
}