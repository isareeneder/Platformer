using UnityEngine;

public class PickUps : MonoBehaviour
{

    [SerializeField] private int pickupValue = 100;

    [SerializeField] private AudioClip coinPickUpSFX;

    bool wasCollected = false;


    private void OnTriggerEnter2D(Collider2D collision)
   {

        if(wasCollected)
        {
            return; 
        }

        wasCollected = true;

        FindFirstObjectByType<GameSession>().ProcessPlayerScore(pickupValue);

        AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position, 0.5f);
        
        Destroy (gameObject);
   }
}
