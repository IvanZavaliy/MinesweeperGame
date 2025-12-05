using System.IO;
using TMPro;
using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private DatabaseManager dbManager;

    private const string SaveFileName = "playerData.json";

    [System.Serializable]
    private class PlayerData
    {
        public string nickname;
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
                Debug.Log("Nickname Saved!");
            }
            catch (System.Exception e)
            {
                Debug.Log($"Error saving player data: {e.Message}");
            }
        }
    }
}
