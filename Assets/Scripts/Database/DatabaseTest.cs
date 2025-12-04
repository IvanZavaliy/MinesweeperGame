using System;
using Npgsql;
using UnityEngine;

public class DatabaseTest : MonoBehaviour
{
    public static void postgres()
    {
        string connectionString = 
            "Host=localhost;" +
            "Username=postgres;" +
            "Password=2955;" +
            "Database=postgres;";

        NpgsqlConnection dbcon;
        
        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();
        
        NpgsqlCommand dbcmd = dbcon.CreateCommand();
        
        string sql =
            "SELECT * " +
            "FROM test_table";
        dbcmd.CommandText = sql;
        //IDataReader reader = dbcmd.ExecuteReader(); ## CHANGE THIS TO
        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            //string FirstName = (string)reader.GetString(0); 
            string LastName = (reader.IsDBNull(1)) ? "NULL" : reader.GetString(1).ToString();
            //string LastName = (string)reader.GetString(1);
            Debug.Log("Name: " + " " + LastName);
            //Console.WriteLine();
        }
        // clean up
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }

    private void Start()
    {
        postgres();
    }
}
