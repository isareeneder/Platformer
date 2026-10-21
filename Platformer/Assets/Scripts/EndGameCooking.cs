using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Added for level transition
using TMPro;

public class EndGameChest : MonoBehaviour
{
    [Header("UI & Audio")]
    [SerializeField] private TextMeshProUGUI depositText;   // Drag your UI Text here
    [SerializeField] private AudioSource celebrateAudio;     // Drag your AudioSource here

    [Header("Timing")]
    [SerializeField] private float delayBetweenCounts = 0.2f; // Speed of the number ticking up
    [SerializeField] private float delayBeforeExit = 2.5f;    // How long to celebrate before leaving

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger once, and only when the Player touches it
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            // Grabs the score safely from your GameSession instance
            if (GameSession.instance != null)
            {
                int totalPickups = GameSession.instance.PlayerScore; 
                StartCoroutine(EndSequence(totalPickups));
            }
            else
            {
                Debug.LogError("EndGameChest: Could not find an active GameSession in the scene!");
                TriggerGameExit(); // Fallback exit if GameSession is missing
            }
        }
    }

    private IEnumerator EndSequence(int finalCount)
    {
        depositText.gameObject.SetActive(true);

        // 1. Tick the numbers up visually from 0 to your player's actual score
        for (int i = 0; i <= finalCount; i++)
        {
            depositText.text = "Mushrooms Cooked: " + i;
            yield return new WaitForSeconds(delayBetweenCounts);
        }

        // 2. Text update & Play celebratory noise
        depositText.text = $"All {finalCount} Mushrooms Have Been Added To The Soup!";
        if (celebrateAudio != null)
        {
            celebrateAudio.Play();
        }

        // 3. Wait for the celebration to finish
        yield return new WaitForSeconds(delayBeforeExit);

        // 4. Trigger the level transition
        TriggerGameExit();
    }

    private void TriggerGameExit()
    {
        // Grabs the current scene's index number and loads the next numerical level index in your Build Settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Safety check to verify a next scene actually exists in Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels found! Returning to Main Menu (Scene 0).");
            SceneManager.LoadScene(0); 
        }
    }
}
