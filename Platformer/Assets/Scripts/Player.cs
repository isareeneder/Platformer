using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

[SerializeField] private float runSpeed = 20.0f;
    
    [SerializeField] private InputActionAsset inputActions;
   
    InputAction moveAction;

    public Vector2 MoveInput { get; private set; }

    Rigidbody2D playerCharacter; 

    // Initializes its contents before the game begins
    void Awake ()
    {
        playerCharacter = GetComponent<Rigidbody2D>();
       
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
        MoveInput = moveAction.ReadValue<Vector2>();

        Run();

    }

    private void Run()
    {
        float hMovement = MoveInput.x;

        playerCharacter.linearVelocity = new Vector2(hMovement * runSpeed, playerCharacter.linearVelocity.y);

    }

}
