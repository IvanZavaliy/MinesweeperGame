using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UI;
using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private DatabaseManager dbManager;

    [SerializeField] private MenuCanvasView view;

    private const string SaveFileName = "playerData.json";
    
    private class PlayerData
    {
        public string nickname;
    }

    private async void Start()
    {
        string nickname = await GetNicknameFromJson();
        if (!string.IsNullOrEmpty(nickname))
        {
            view.UpdateNicknameText(nickname);
        }
        else
        {
            view.UpdateNicknameText("");
        }
    }

    public async void SaveNicknameToJson()
    {
        string nicknameToSave =  nicknameInput.text;

        bool isTaken = await dbManager.IsNicknameTaken(nicknameToSave);

        if (isTaken)
        {
            Debug.Log("Error! Nickname is taken");
        }
        else
        {
            if (string.IsNullOrEmpty(nicknameToSave))
            {
                return;
            }
            
            PlayerData data = new PlayerData();
            data.nickname = nicknameToSave;
        
            string json = JsonUtility.ToJson(data, true);

            string path = Path.Combine(Application.persistentDataPath, SaveFileName);

            try
            {
                await File.WriteAllTextAsync(path, json);
                CanvasUpdate(nicknameToSave);
                Debug.Log("Nickname Saved!");
            }
            catch (System.Exception e)
            {
                Debug.Log($"Error saving player data: {e.Message}");
            }
        }
    }

    private void CanvasUpdate(string nickname)
    {
        view.UpdateNicknameText(nickname);
        nicknameInput.text = string.Empty;
        view.ShowInformationText("Nickname Saved!", Color.forestGreen);
    }

    public bool NicknameAvailabilityCheck()
    {
        if (view.nicknameTextLabel.text == string.Empty || nicknameInput.text == "Name")
        {
            view.ShowInformationText("Nickname not entered!", Color.red);
            return false;
        }
        
        return true;
    }

    public static async Task<string> GetNicknameFromJson()
    {
        string nicknameDirectoryPath = Path.Combine(Application.persistentDataPath, SaveFileName);

        if (!File.Exists(nicknameDirectoryPath))
        {
            return string.Empty;
        }

        try
        {
            string json = await File.ReadAllTextAsync(nicknameDirectoryPath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }

            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            if (data != null && !string.IsNullOrEmpty(data.nickname))
            {
                return data.nickname;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading player data: {e.Message}");
        }
        
        return string.Empty;
    }
}
