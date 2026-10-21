using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    // Drag your UI Text GameObject into this slot in the Inspector
    public GameObject uiText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // This will print a message in your Unity Console window as soon as ANYTHING touches it
        Debug.Log("Something entered the trigger zone: " + other.name);

        // Check if the object entering the zone is the Player
        if (other.CompareTag("Player"))
        {
            uiText.SetActive(true); // Show the text
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Hide the text again when the player leaves the zone
        if (other.CompareTag("Player"))
        {
            uiText.SetActive(false); // Hide the text
        }
    }
}
