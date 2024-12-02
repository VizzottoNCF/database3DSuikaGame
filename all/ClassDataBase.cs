using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Security.Cryptography;
using UnityEngine.UI;
using System;

public class ClassDataBase : MonoBehaviour
{
    private IDbConnection conec; // faz a conexão do banco de dados ao jogo
    private IDbCommand command; // comanda o SQL através do connection
    private IDataReader reader; // lê os dados do command

    //private string StringConexao = "URI=file:Assets/StreamingAssets/meuBanco.db";
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
            command.CommandText = "PRAGMA foreign_keys = ON;"+"CREATE TABLE IF NOT EXISTS USUARIOS" + "(ID INTEGER PRIMARY KEY AUTOINCREMENT, NOME VARCHAR(50) NOT NULL UNIQUE, HIGHSCORE INTEGER);";

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

    public bool inserir(string _nome)
    {
        try
        {
            // conecta com o banco de dados
            conectar();

            // insere usuario na tabela
            //command.CommandText = "INSERT INTO USUARIOS(NOME) VALUES('" + _nome + "')"; // SOFRE DE SQL INJECTION
            command.CommandText = "INSERT INTO USUARIOS(NOME) VALUES($_nome)";

            // coloca o usuario atraves de um parametro para evitar SQL injection
            command.Parameters.Add(_nome);

            // executa o CommandText
            command.ExecuteNonQuery();

            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
        finally {conec.Close();}
    }

    public bool inserirScalar(string _nome)
    {
        try
        {
            conectar();

            long idInserido;

            //string comandoSql = "INSERT INTO USUARIOS(NOME) VALUES ('" + _nome + "'); SELECT CAST(scope_identity() AS int)";
            string comandoSql = "INSERT INTO USUARIOS(NOME) VALUES ('" + _nome + "'); SELECT last_insert_rowid()";

            command.CommandText = comandoSql;
            //print("ANTES");

            idInserido = Convert.ToInt32((long)command.ExecuteScalar());

            //print("DEPOIS");
            Debug.Log(idInserido);
            // inserir em uma tabela que tenha FK

            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
        finally {conec.Close();}
    }
    public bool remover(int _id)
    {
        try
        {
            conectar();

            command.CommandText = "delete from usuarios where id = " + _id;
            command.ExecuteNonQuery();

            return true;
        }
        catch  (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
        finally {conec.Close();}
    }

    public bool alterar(int _id, string _nome)
    {
        try
        {
            conectar();

            command.CommandText = "update usuarios set nome = '" + _nome + "' where id = " + _id;
            command.ExecuteNonQuery();

            return true;
        }
        catch (System.Exception ex)
        {
            print (ex.Message);
            return false;
        }
        finally {conec.Close();}
    }

    public bool consultar()
    {
        try
        {
            conectar();

            command.CommandText = "select * from usuarios;";

            reader = command.ExecuteReader();

            int cont = 0;

            while (reader.Read())
            {
                print("ID:" + reader.GetInt32(0) + " | NOME: " +  reader.GetString(1));

                cont++;
            }

            print("Registros encontrados: " + cont);

            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
        finally {conec.Close();}
    }
}
