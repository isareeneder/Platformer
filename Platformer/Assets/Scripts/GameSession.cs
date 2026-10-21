using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    public static GameSession instance; // this was added

    [Header("Player Counts")]
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] float deathDelay = 2.0f;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI score;

    void Awake()
    {
        int numGameSessions = FindObjectsByType<GameSession>().Length;

        if (numGameSessions > 1)
        {
            Destroy(gameObject);
            return; // this was added
        }
        else
        {
            instance = this; // this was added
            DontDestroyOnLoad(gameObject);
        }
    }

    public int PlayerScore => playerScore; // this was added

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            playerScore = 0;
            playerLives = 3;
            RefreshUI();
            Debug.Log("GameSession: Level 1 detected. Score reset to 0!");
        }
    }

    void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (lives != null) lives.text = playerLives.ToString();
        if (score != null) score.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        StartCoroutine(DeathSequence());
    }

    public void ProcessPlayerScore(int points)
    {
        playerScore += points;
        RefreshUI();
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSecondsRealtime(deathDelay);

        if (playerLives > 1)
        {
            SubtractLife();
        }
        else
        {
            ResetGameSession();
            playerLives = 3;
        }
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0); 
    }

    private void SubtractLife()
    {
        playerLives--;
        RefreshUI();

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
