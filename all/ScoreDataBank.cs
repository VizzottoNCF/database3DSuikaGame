using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Security.Cryptography;
using UnityEngine.UI;
using System;

public class ScoreDataBank : MonoBehaviour
{
    private IDbConnection conec; // faz a conexão do banco de dados ao jogo
    private IDbCommand command; // comanda o SQL através do connection
    private IDataReader reader; // lê os dados do command

    private string StringConexao = "URI=file:Assets/StreamingAssets/Scores.db";

    private bool conectar()
    {
        try
        {
            conec = new SqliteConnection(StringConexao); // cria a conexão, se o banco não existe, cria ele
            conec.Open(); // abriu conexão com o banco de dados

            // cria o comando atraves do connect
            command = conec.CreateCommand();

            // enables foreign keys, then creates a user table if it does not exist
            command.CommandText = "PRAGMA foreign_keys = ON;" + "CREATE TABLE IF NOT EXISTS JOGADORES" + "(ID INTEGER PRIMARY KEY AUTOINCREMENT, NOME VARCHAR(50) NOT NULL UNIQUE, HIGHSCORE INTEGER DEFAULT 0);";

            // executa o command.CommandText
            command.ExecuteNonQuery();

            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
    }

    public bool insertPlayer(string _username)
    {
        try
        {
            conectar();

            command.CommandText = "SELECT * FROM JOGADORES";

            reader = command.ExecuteReader();

            // cycles through users
            while (reader.Read())
            {
                print("ID:" + reader.GetInt32(0) + " | USERNAME: " + reader.GetString(1) + " | HIGHSCORE: " + reader.GetInt32(2));

                // checks for current user
                if (reader.GetString(1) == _username)
                {
                    // Player already exists
                    reader.Close();
                    return true;
                }
            }
            
            reader.Close();
            long idInserido;

            string comandoSql = "INSERT INTO JOGADORES(NOME) VALUES ('" + _username + "'); SELECT last_insert_rowid()";


            command.CommandText = comandoSql;

            idInserido = Convert.ToInt32((long)command.ExecuteScalar());
            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
        finally { conec.Close(); }
    }
    public bool insertHighScore(string _username, int _highscore)
    {
        try
        {
            // commented because it's called in the middle of another connection
            //conectar();

            // updates your highscore 
            string comandoSql = "UPDATE JOGADORES SET HIGHSCORE = " + _highscore + " WHERE NOME = '" + _username + "'";
            command.CommandText = comandoSql;

            command.ExecuteNonQuery();
            print("UPDATED HIGHSCORE");

            
            command.CommandText = "SELECT * FROM JOGADORES WHERE NOME = '" + _username + "'";
            command.ExecuteNonQuery();
            

            return true;
        }
        catch (System.Exception ex)
        {
            print (ex.Message);
            return false;
        }
        finally { /*conec.Close();*/ }
    }

    public bool checkForHighScore (string _username, int _highscore)
    {
        try
        {
            conectar();

            command.CommandText = "SELECT * FROM JOGADORES";

            reader = command.ExecuteReader();

            // cycles through users
            while (reader.Read())
            {
                print("ID:" + reader.GetInt32(0) + " | USERNAME: " + reader.GetString(1) + " | HIGHSCORE: " + reader.GetInt32(2));

                // checks for current user
                if (reader.GetString(1) == _username)
                {
                    // checks if last score is higher than your high score
                    if (reader.GetInt32(2) < _highscore)
                    {
                        // calls insert highscore function
                        reader.Close();
                        insertHighScore(_username, _highscore);
                    }
                }
            }
            if (!reader.IsClosed) { reader.Close(); }

            return true;
        }
        catch (System .Exception ex)
        {
            print(ex.Message);
            return false;
        }
        finally { conec.Close(); }
    }
}
