using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public GameObject uiText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger zone: " + other.name);

        if (other.CompareTag("Player"))
        {
            uiText.SetActive(true); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiText.SetActive(false);
        }
    }
}
