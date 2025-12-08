using System;
using System.Threading.Tasks;
using Npgsql;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DatabaseManager : MonoBehaviour
{
    [Header("UI Containers (Vertical Layout Groups) FOR TESTING")]
    [SerializeField] private Transform rankContainer;
    [SerializeField] private Transform nicknameContainer;
    [SerializeField] private Transform timeContainer;
    [SerializeField] private Transform attemptsContainer;
    
    [Header("Prefabs FOR TESTING")]
    public GameObject numberTextPrefab;  
    public GameObject nicknameTextPrefab;
    
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public Difficulty difficulty;

    private string TableNameForPutRequest
    {
        get
        {
            return GetTableName(difficulty);
        }
    }

    private readonly string _connectionString = 
        "Host=localhost;" +
        "Username=postgres;" +
        "Password=2955;" +
        "Database=postgres;";

    private void Start()
    {
        UpdateLeaderboard(Difficulty.Easy);
    }

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
        ClearLeaderboard();
        
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

                            CreateRow(currentRank, nickname, time, attempts);

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
    
    private void CreateRow(int rank, string nickname, int time, int attempts)
    {
        CreateTextObject(rankContainer, numberTextPrefab, rank + ".");
        CreateTextObject(nicknameContainer, nicknameTextPrefab, nickname);
        CreateTextObject(timeContainer, numberTextPrefab, time + "s");
        CreateTextObject(attemptsContainer, numberTextPrefab, attempts.ToString());
    }

    private void CreateTextObject(Transform container, GameObject prefab, string content)
    {
        GameObject newObject = Instantiate(prefab, container);
        TextMeshProUGUI text = newObject.GetComponent<TextMeshProUGUI>();
        
        text.text = content;
    }

    private void ClearLeaderboard()
    {
        ClearContainer(rankContainer);
        ClearContainer(nicknameContainer);
        ClearContainer(timeContainer);
        ClearContainer(attemptsContainer);
    }

    private void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
    
    private string GetTableName(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.Easy: return "minefield_easy_leaderboard";
            case Difficulty.Medium: return "minefield_medium_leaderboard";
            case Difficulty.Hard: return "minefield_hard_leaderboard";
            default: return "minefield_easy_leaderboard";
        }
    }
}
