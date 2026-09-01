using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    InputAction moveAction;

    // Initializes its contents before the game begins
    void Awake ()
    {
        InputActionMap playerMap = inputActions.FindActionMap("Player", true);

        moveAction = playerMap.FindAction("Move", true);

        playerMap.Enable();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
