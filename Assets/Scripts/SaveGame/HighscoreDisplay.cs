using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighscoreDisplay : MonoBehaviour
{
    public static HighscoreDisplay Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject highscoreEntryPrefab;
    [SerializeField] private Transform contentParent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  
        }
        else
        {
            Instance = this;  
        }
    }

    private void Start()
    {
        DisplayHighscores();
    }

    public void DisplayHighscores()
    {
        List<KeyValuePair<string, int>> sortedHighscores = HighscoreManager.Instance.GetSortedHighscores();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var highscore in sortedHighscores)
        {
            GameObject entry = Instantiate(highscoreEntryPrefab, contentParent);

            TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = highscore.Key;  
                texts[1].text = highscore.Value.ToString();  
            }
        }
    }

    public void ClearHighScore()
    {
        HighscoreManager.Instance.ClearHighscores();
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }


}
