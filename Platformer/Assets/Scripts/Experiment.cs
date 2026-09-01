using UnityEngine;

public class Experiment : MonoBehaviour
{

[SerializeField] string playerName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
