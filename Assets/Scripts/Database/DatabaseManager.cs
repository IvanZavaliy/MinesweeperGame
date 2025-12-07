using System;
using System.Threading.Tasks;
using Npgsql;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public enum TableDifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public TableDifficultyLevel tableDifficultyLevel;

    private string TableName
    {
        get
        {
            switch (tableDifficultyLevel)
            {
                case TableDifficultyLevel.Easy: return "minefield_easy_leaderboard";
                case TableDifficultyLevel.Medium: return "minefield_medium_leaderboard";
                case TableDifficultyLevel.Hard: return "minefield_hard_leaderboard";
                default: return null;
            }
        }
    }
    
    string connectionString = 
        "Host=localhost;" +
        "Username=postgres;" +
        "Password=2955;" +
        "Database=postgres;";

    public async Task<bool> IsNicknameTaken(string nickname)
    {
        string sqlQuery = "SELECT COUNT(*) " +
                          $"FROM players " +
                          "WHERE nickname = @name";

        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
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
}
