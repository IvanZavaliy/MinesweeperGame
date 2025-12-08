using System;
using System.Threading.Tasks;
using Npgsql;
using UnityEngine;

public class DatabaseUpdate : MonoBehaviour
{
    private readonly string _connectionString = 
        "Host=localhost;" +
        "Username=postgres;" +
        "Password=2955;" +
        "Database=leaderboards;";
    
    public Difficulty difficulty;
    
    private string TableNameForPutRequest => DatabaseManager.GetTableName(difficulty);

    private string PlayerTablePutSqlRequest(string playerName)
    {
        return "INSERT INTO players (nickname) " +
               $"VALUES ('{playerName}') " +
               "ON CONFLICT (nickname) DO NOTHING;";
    }

    public async void UpdatePlayerAttempts()
    {
        
        string tableName = TableNameForPutRequest;
        string playerName = await PlayerDataSaver.GetNicknameFromJson();
            
        string sql = PlayerTablePutSqlRequest(playerName) +
                     $"INSERT INTO {tableName} (nickname, time, attempts_count)" +
                     "VALUES (@nick, @time, @attempts)" +
                     "ON CONFLICT (nickname) " +
                     "DO UPDATE SET" +
                     $"    time = LEAST({tableName}.time, EXCLUDED.time)," +
                     $"    attempts_count = {tableName}.attempts_count + 1;";
        
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
    
                using (var cmd = new NpgsqlCommand(sql, connection)) 
                {
                    cmd.Parameters.AddWithValue("nick", playerName);
                    cmd.Parameters.AddWithValue("time", int.MaxValue);
                    cmd.Parameters.AddWithValue("attempts", 1);
                
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            
            Debug.Log("The result was successfully sent to the DB!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending to DB: {e.Message}");
        }
    }

    public async void SaveWinPlayerResult(int winTime)
    {
        string tableName = TableNameForPutRequest;
        string playerName = await PlayerDataSaver.GetNicknameFromJson();
        
        string sql = PlayerTablePutSqlRequest(playerName) + 
                     $"INSERT INTO {tableName} (nickname, time, attempts_count)" + 
                     "VALUES (@nick, @time, 1)" + 
                     "ON CONFLICT (nickname) " + 
                     "DO UPDATE SET" + 
                     $" time = LEAST({tableName}.time, EXCLUDED.time), " +
                     $" attempts_count = {tableName}.attempts_count + 1;";
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("nick", playerName);
                    cmd.Parameters.AddWithValue("time", winTime);
                    
                    await cmd.ExecuteNonQueryAsync();
                    Debug.Log("Database updated!");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving winnings: {e.Message}");
        }
    }
}
