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

            void SendCommand(string cmd)
            {
                Command.CommandText = cmd;
                Command.ExecuteNonQuery();
            }

            // Creating the Saves Table
            SendCommand("CREATE TABLE IF NOT EXISTS Saves (SaveNum INTEGER, HasStarted INTEGER, PlayerName TEXT, PlayerSkin INTEGER);");
            // Creating the Items Table
            SendCommand("CREATE TABLE IF NOT EXISTS Items (SaveNum INTEGER, WoodAmount INTEGER, SteelAmount INTEGER, ElectronicsAmount INTEGER, FOREIGN KEY(SaveNum) REFERENCES Saves(SaveNum));");
            // Creating the Settings Table
            SendCommand("CREATE TABLE IF NOT EXISTS Settings (SaveNum INTEGER,  Volume FLOAT, PlayerSpeed INTEGER, EnemyDamage INTEGER, FOREIGN KEY(SaveNum) REFERENCES Saves(SaveNum));");

            #region Initial Writes to Tables

            // Initial Write for Save Table
            SendCommand("INSERT OR REPLACE INTO Saves ('SaveNum', 'HasStarted', 'PlayerName', 'PlayerSkin') VALUES (1, '0', '', 0);");
            SendCommand("INSERT OR REPLACE INTO Saves ('SaveNum', 'HasStarted', 'PlayerName', 'PlayerSkin') VALUES (2, '0', '', 0);");
            SendCommand("INSERT OR REPLACE INTO Saves ('SaveNum', 'HasStarted', 'PlayerName', 'PlayerSkin') VALUES (3, '0', '', 0);");

            // Initial Writes for Items Table
            SendCommand("INSERT OR REPLACE INTO Items ('SaveNum', 'WoodAmount', 'SteelAmount', 'ElectronicsAmount') VALUES (1, 0, 0, 0);");
            SendCommand("INSERT OR REPLACE INTO Items ('SaveNum', 'WoodAmount', 'SteelAmount', 'ElectronicsAmount') VALUES (2, 0, 0, 0);");
            SendCommand("INSERT OR REPLACE INTO Items ('SaveNum', 'WoodAmount', 'SteelAmount', 'ElectronicsAmount') VALUES (3, 0, 0, 0);");

            // Initial Writes for Settings Table
            SendCommand("INSERT OR REPLACE INTO Settings ('SaveNum', 'Volume', 'PlayerSpeed', 'EnemyDamage') VALUES (1, 1.0, 1, 1);");
            SendCommand("INSERT OR REPLACE INTO Settings ('SaveNum', 'Volume', 'PlayerSpeed', 'EnemyDamage') VALUES (2, 1.0, 1, 1);");
            SendCommand("INSERT OR REPLACE INTO Settings ('SaveNum', 'Volume', 'PlayerSpeed', 'EnemyDamage') VALUES (3, 1.0, 1, 1);");

            #endregion

            Connection.Close();
        }
    }

    public string Read(string table, string column, int saveNum)
    {
        using (SqliteConnection Connection = new SqliteConnection(dbName))
        {
            Connection.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT " + column + " FROM " + table + " WHERE SaveNum = " + saveNum.ToString(), Connection);
            SqliteDataReader reader = cmd.ExecuteReader();

            // Making a string that we return instead so that we can close the Connection
            string returnValue = reader.GetValue(0).ToString();
            Connection.Close();

            return returnValue;
        }
    }

    public void Write(string table, string column, int saveNum, string variableToUse)
    {
        using (var Connection = new SqliteConnection(dbName))
        {
            Connection.Open();

            //Setting up an object command to allow db control
            using (var command = Connection.CreateCommand())
            {
                // Selecting the relevant cell
                command.CommandText = "SELECT DISTINCT " + column + " FROM " + table + " WHERE SaveNum = " + saveNum.ToString() + " ORDER BY SaveNum;";
                command.ExecuteNonQuery();
                // Updating the relevant cell
                command.CommandText = "UPDATE " + table + " SET " + column + " = " + variableToUse + " WHERE SaveNum = " + saveNum.ToString();
                command.ExecuteNonQuery();
            }
            Connection.Close();
        }
    }

    public void DeleteSaveFile(int saveNum)
    {
        Write("Saves", "HasStarted", saveNum, "0");
        Write("Saves", "PlayerName", saveNum, "''");
        Write("Saves", "PlayerSkin", saveNum, "0");

        Write("Items", "WoodAmount", saveNum, "0");
        Write("Items", "SteelAmount", saveNum, "0");
        Write("Items", "ElectronicsAmount", saveNum, "0");

        Write("Settings", "Volume", saveNum, "1");
        Write("Settings", "PlayerSpeed", saveNum, "1");
        Write("Settings", "EnemyDamage", saveNum, "1");
    }
}