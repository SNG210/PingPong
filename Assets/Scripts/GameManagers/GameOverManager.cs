using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Error Message Settings")]
    [SerializeField] private float errorMessageDuration = 1f;

    public void UpdateFinalScore()
    {
        finalScoreText.text = $"Score: {GameManager.Instance.GetScore()}";
        errorMessageText.gameObject.SetActive(false);
    }
    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitScore);
    }

    private void OnSubmitScore()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            ShowErrorMessage("Name cannot be empty!");
            return;
        }

        if (HighscoreManager.Instance.DoesPlayerExist(playerName))
        {
            ShowErrorMessage($"Player name '{playerName}' already exists! Choose a different name.");
            return;
        }

        int playerScore = GameManager.Instance.GetScore();
        HighscoreManager.Instance.AddHighscore(playerName, playerScore);

        GameManager.Instance.ClearTemporaryScore();

        Debug.Log($"Highscore submitted for {playerName}: {playerScore}");
        UiPanelManager.Instance.ActivatePanel("HighScore");
    }
    private void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        StartCoroutine(nameof(HideErrorMessage));
       
    }

    private IEnumerator HideErrorMessage()
    {
        yield return new WaitForSecondsRealtime(errorMessageDuration);
        errorMessageText.gameObject.SetActive(false);
    }

}


