
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    private static ScenePersist instance;

    private int startingSceneIndex;

    void Awake()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance.startingSceneIndex != this.startingSceneIndex)
            {
                Destroy(instance.gameObject);

                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
