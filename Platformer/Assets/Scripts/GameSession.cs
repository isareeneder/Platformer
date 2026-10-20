using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameSession : MonoBehaviour
{

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

        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        RefreshUI();
    }
    
    private void RefreshUI()
    {
        lives.text = playerLives.ToString();
        score.text = playerScore.ToString();
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
        
        if(playerLives > 1)
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
