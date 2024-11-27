using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    [SerializeField] private float resetDelay = 1f;
    [SerializeField] private float launchDelay = 1f;

    [Header("UI Elements")]
    [SerializeField] private GameObject countdownPanel;
    private TMP_Text countdownText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject pauseMenuUI;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private AIOpponent aiOpponent;
    [SerializeField] private Ball ball;

    private int score = 0;
    private bool isPaused = false;

    public delegate void GameOverEvent();
    public static event GameOverEvent OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        pauseMenuUI.SetActive(false);

        countdownText = countdownPanel.GetComponentInChildren<TMP_Text>();

        StartGameCountdown();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        ResetGameAfterScore();
    }

    public void TriggerGameOver()
    {
        OnGameOver?.Invoke();
        UiPanelManager.Instance.ActivatePanel("GameOver");
        PauseGame(true);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void ResetGameAfterScore()
    {
        PauseGame(true);
        StartCoroutine(ResetGameCoroutine());
    }

    private IEnumerator ResetGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(resetDelay);
        ResetPositions();

        yield return new WaitForSecondsRealtime(launchDelay);
        PauseGame(false);
        ball.LaunchBall();
    }

    private void ResetPositions()
    {
        ball.ResetPosition();
        player.transform.position = player.GetPlayerRestPosition();
        aiOpponent.transform.position = aiOpponent.GetAiRestPosition();
    }

    private void StartGameCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        PauseGame(true);
        ResetPositions();
        ClearTemporaryScore();
        countdownPanel.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }
        
        PauseGame(false);
        player.allowMovement = true;
        ball.LaunchBall();
    }

    public void PauseGame(bool shouldPause)
    {
        isPaused = shouldPause;
        Time.timeScale = shouldPause ? 0f : 1f;
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        SetCursorState(isPaused);
        pauseMenuUI.SetActive(isPaused);
        PauseGame(isPaused);
    }


    public int GetScore()
    {
        return score;
    }

    public void ClearTemporaryScore()
    {
        score = 0;
        UpdateScoreText(); 
    }

    private void SetCursorState(bool unlockCursor)
    {
        Cursor.lockState = unlockCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = unlockCursor;
    }
}
