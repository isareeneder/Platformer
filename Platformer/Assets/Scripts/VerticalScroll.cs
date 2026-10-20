using UnityEngine;

public class VerticalScroll : MonoBehaviour
{

    [Tooltip("Game Units Per Second")]
    [SerializeField] private float scrollSpeed = 0.2f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2 (0.0f, scrollSpeed * Time.deltaTime));
    }
}
