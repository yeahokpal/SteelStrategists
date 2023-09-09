using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEditor.MemoryProfiler;

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
        if (!File.Exists("Database.db"))
            CreateDB();
    }

    public void CreateDB()
    {
        // Create DB connection
        using (var Connection = new SqliteConnection(dbName))
        {
            Connection.Open();

            // Set up an object command to control database
            IDbCommand Command = Connection.CreateCommand();

            // Creating the Saves Table
            Command.CommandText = "CREATE TABLE IF NOT EXISTS Saves (SaveNum INTEGER, HasStarted INTEGER, PlayerName TEXT, PlayerSkin INTEGER);";
            Command.ExecuteNonQuery();

            // Creating the Items Table
            Command.CommandText = "CREATE TABLE IF NOT EXISTS Items (SaveNum INTEGER, WoodAmount INTEGER, SteelAmount INTEGER, ElectronicsAmount INTEGER, FOREIGN KEY(SaveNum) REFERENCES Saves(SaveNum));";
            Command.ExecuteNonQuery();

            // Creating the Settings Table
            Command.CommandText = "CREATE TABLE IF NOT EXISTS Settings (SaveNum INTEGER, Volume FLOAT, PlayerSpeed INTEGER, EnemyDamage INTEGER, FOREIGN KEY(SaveNum) REFERENCES Saves(SaveNum));";
            Command.ExecuteNonQuery();

            #region Initial Writes to Tables

            // Initial Write for Save Table

            Command.CommandText = "INSERT OR REPLACE INTO Saves ('SaveNum', 'HasStarted', 'PlayerName', 'PlayerSkin') VALUES (1, '0', '', 0);";
            Command.ExecuteNonQuery();

            Command.CommandText = "INSERT OR REPLACE INTO Saves ('SaveNum', 'HasStarted', 'PlayerName', 'PlayerSkin') VALUES (2, '0', '', 0);";
            Command.ExecuteNonQuery();

            Command.CommandText = "INSERT OR REPLACE INTO Saves ('SaveNum', 'HasStarted', 'PlayerName', 'PlayerSkin') VALUES (3, '0', '', 0);";
            Command.ExecuteNonQuery();

            // Initial Writes for Items Table

            Command.CommandText = "INSERT OR REPLACE INTO Items ('SaveNum', 'WoodAmount', 'SteelAmount', 'ElectronicsAmount') VALUES (1, 0, 0, 0);";
            Command.ExecuteNonQuery();

            Command.CommandText = "INSERT OR REPLACE INTO Items ('SaveNum', 'WoodAmount', 'SteelAmount', 'ElectronicsAmount') VALUES (2, 0, 0, 0);";
            Command.ExecuteNonQuery();

            Command.CommandText = "INSERT OR REPLACE INTO Items ('SaveNum', 'WoodAmount', 'SteelAmount', 'ElectronicsAmount') VALUES (3, 0, 0, 0);";
            Command.ExecuteNonQuery();

            // Initial Writes for Settings Table

            Command.CommandText = "INSERT OR REPLACE INTO Settings ('SaveNum', 'Volume', 'PlayerSpeed', 'EnemyDamage') VALUES (1, 1.0, 1, 1);";
            Command.ExecuteNonQuery();

            Command.CommandText = "INSERT OR REPLACE INTO Settings ('SaveNum', 'Volume', 'PlayerSpeed', 'EnemyDamage') VALUES (2, 1.0, 1, 1);";
            Command.ExecuteNonQuery();

            Command.CommandText = "INSERT OR REPLACE INTO Settings ('SaveNum', 'Volume', 'PlayerSpeed', 'EnemyDamage') VALUES (3, 1.0, 1, 1);";
            Command.ExecuteNonQuery();

            #endregion

            Connection.Close();
        }
    }
}