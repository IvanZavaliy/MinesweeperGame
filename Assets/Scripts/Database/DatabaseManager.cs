using System;
using System.Threading.Tasks;
using Npgsql;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] private MenuCanvasView view;

    private readonly string _connectionString = 
        "Host=localhost;" +
        "Username=postgres;" +
        "Password=2955;" +
        "Database=leaderboards;";

    public async Task<bool> IsNicknameTaken(string nickname)
    {
        string sqlQuery = "SELECT (nickname, time, attempts_count) " +
                          "FROM players " +
                          "WHERE nickname = @name";

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@name", nickname);

                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                    {
                        long count = Convert.ToInt64(result);
                        return count > 0;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Database error: " + e.Message);
            return false;
        }
        
        return false;
    }

    public async void UpdateLeaderboard(Difficulty difficulty)
    {
        view.ClearLeaderboard();
        
        string tableName = GetTableName(difficulty);
        string sql = "SELECT *" +
                     $"FROM {tableName} " +
                     "ORDER BY time " +
                     "ASC LIMIT 10";
        
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int currentRank = 1;

                        while (await reader.ReadAsync())
                        {
                            string nickname = reader.GetString(0);
                            int time = reader.GetInt32(1);
                            int attempts = reader.GetInt32(2);
                                
                            view.CreateRow(currentRank, nickname, time, attempts);

                            currentRank++;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error leaderboard loading: " + e.Message);
        }
    }
    
    public static string GetTableName(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.Easy: return "minefield_easy_leaderboard";
            case Difficulty.Medium: return "minefield_medium_leaderboard";
            case Difficulty.Hard: return "minefield_hard_leaderboard";
            case Difficulty.None: return string.Empty;
            default: return "minefield_easy_leaderboard";
        }
    }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    None
}