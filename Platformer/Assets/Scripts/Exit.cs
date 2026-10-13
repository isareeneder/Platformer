using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class Exit : MonoBehaviour
{
    [SerializeField] float levelloadDelay = 0.1f;
    //[SerializeField] float slowMoFactor = 0.2f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        
        //Time.timeScale = slowMoFactor;
        
        yield return new WaitForSecondsRealtime(levelloadDelay);

        //Time.timeScale = 1.0f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
