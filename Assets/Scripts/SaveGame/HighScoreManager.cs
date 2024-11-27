using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance; 

    private const string HighscoreKey = "LocalHighscores"; 
    private Dictionary<string, int> highscoreDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }

        LoadHighscores();
    }

    private void LoadHighscores()
    {
        if (PlayerPrefs.HasKey(HighscoreKey))
        {
            string json = PlayerPrefs.GetString(HighscoreKey);
            highscoreDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }
        else
        {
            highscoreDictionary = new Dictionary<string, int>();
        }
    }

    public void AddHighscore(string playerName, int score)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name cannot be empty.");
            return;
        }

        if (highscoreDictionary.ContainsKey(playerName))
        {
            if (score > highscoreDictionary[playerName])
            {
                highscoreDictionary[playerName] = score;
                Debug.Log($"Updated highscore for {playerName}: {score}");
            }
        }
        else
        {
            highscoreDictionary.Add(playerName, score);
            Debug.Log($"Added new highscore for {playerName}: {score}");
        }

        SaveHighscores();
    }

    private void SaveHighscores()
    {
        string json = JsonConvert.SerializeObject(highscoreDictionary, Formatting.Indented);
        PlayerPrefs.SetString(HighscoreKey, json);
        PlayerPrefs.Save();
    }

    public List<KeyValuePair<string, int>> GetSortedHighscores()
    {
        List<KeyValuePair<string, int>> sortedScores = new List<KeyValuePair<string, int>>(highscoreDictionary);
        sortedScores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)); 
        return sortedScores;
    }

    public void ClearHighscores()
    {
        highscoreDictionary.Clear();
        PlayerPrefs.DeleteKey(HighscoreKey);
        Debug.Log("Cleared all highscores.");
    }

    public bool DoesPlayerExist(string playerName)
    {
        return highscoreDictionary.ContainsKey(playerName);
    }
}
