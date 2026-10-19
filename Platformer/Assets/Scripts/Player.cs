using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Player Speed Settings")]
    [SerializeField] private float runSpeed = 5.0f;

    [SerializeField] private float runAcceleration = 30.0f;

    [SerializeField] private float runDeceleration = 40.0f;


    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 5.0f;

    [SerializeField] [Range(0.1f, 1.0f)] private float jumpCutMultiplier = 0.5f;

    [SerializeField] private float coyoteTime = 0.1f;

    [SerializeField] private float jumpBufferTime = 0.1f;

    [SerializeField] private float ladderJumpTime = 0.15f;

    float jumpedOffLadderTimer;

    [SerializeField] private float fallGravityMultiplier = 2.0f;

    [SerializeField] private float climbSpeed = 5.0f;

    float gravityScaleAtStart;

    float lastGroundTime;

    float jumpBufferTimer;

    [Header("Water Settings")]
    [SerializeField] private float inWaterGravity = 0.0001f;

    [Header("Other Settings")]
    [SerializeField] private LayerMask groundLayer;

    LayerMask climbingLayer;
    
    [SerializeField] private Vector2 deathSeq = new Vector2(10f, 10f);

    [SerializeField] private InputActionAsset inputActions;
   
    InputAction moveAction;

    InputAction jumpAction;

    InputAction attackAction;

    public bool JumpPressedThisFrame => jumpAction != null && jumpAction.WasPressedThisFrame();

    public LayerMask GroundLayer => groundLayer.value != 0 ? groundLayer : LayerMask.GetMask("Ground");

    public Vector2 MoveInput { get; private set; }

    Rigidbody2D playerCharacter; 

    Animator playerAnimator;

    CapsuleCollider2D playerBodyCollider;
    
    BoxCollider2D playerFeetCollider;

    bool isAlive = true;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip attackSFX;

    //Attacking stuff
    //[Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 1;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    //Reference to child animator

    [SerializeField] private Animator childAnimator;


    // Initializes its contents before the game begins
    void Awake ()
    {
        playerCharacter = GetComponent<Rigidbody2D>();

        playerAnimator = GetComponentInChildren<Animator>();

        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = playerCharacter.gravityScale;

        climbingLayer = LayerMask.GetMask("Climbing");
       
        InputActionMap playerMap = inputActions.FindActionMap("Player", true);

        moveAction = playerMap.FindAction("Move", true);

        jumpAction = playerMap.FindAction("Jump", true);

        attackAction = playerMap.FindAction("Attack", true);

        playerMap.Enable();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!isAlive)
        {
            return;
        }


        MoveInput = moveAction.ReadValue<Vector2>();

        // Functions for the game to play
        Run();
        Jump();
        FlipSprite();
        Climb();
        BetterGravity();
        Die();
        WaterGravity();


        if (Time.time >= nextAttackTime)
        {

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                childAnimator.SetTrigger("Magic");
                playerAnimator.SetTrigger("Attack");
                Attack();

                //nextAttackTime = Time.time + 1f / attackRate;

            }
        }

    }

    private void Attack()
    {
       

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
     
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            
            Debug.Log("We hit " + enemy.name);
        }

        AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position, 0.5f);

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    private void Run()
    {
        float hMovement = MoveInput.x;

        float targetSpeed = MoveInput.x * runSpeed; 

        float speedChange; 

        if(Mathf.Abs(targetSpeed) > Mathf.Epsilon)
        {
            speedChange = runAcceleration; 
        }

        else
        {
            speedChange = runDeceleration;
        }

        float newSpeed = Mathf.MoveTowards(playerCharacter.linearVelocity.x, targetSpeed, speedChange * Time.deltaTime);

        playerCharacter.linearVelocity = new Vector2(newSpeed, playerCharacter.linearVelocity.y);

        bool hSpeed = Mathf.Abs(playerCharacter.linearVelocity.x) > Mathf.Epsilon; 

        // Stops the run animation while climbing
        playerAnimator.SetBool("run", hSpeed && !playerBodyCollider.IsTouchingLayers(climbingLayer));

    }

    private void FlipSprite()
    {
        bool hMovement = Mathf.Abs(playerCharacter.linearVelocity.x) > Mathf.Epsilon;

        if(hMovement)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerCharacter.linearVelocity.x), 1f);
        }
    }

    private void Jump()
    {
        //if(jumpAction.WasPressedThisFrame())
        {
            //AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position, 0.5f);
        }



        if (jumpAction.WasReleasedThisFrame() && playerCharacter.linearVelocity.y > 0)
        {
            playerCharacter.linearVelocity = new Vector2(playerCharacter.linearVelocity.x, playerCharacter.linearVelocity.y * jumpCutMultiplier);

        }


        bool isGrounded = playerFeetCollider.IsTouchingLayers(GroundLayer) || playerBodyCollider.IsTouchingLayers(climbingLayer) || playerFeetCollider.IsTouchingLayers(climbingLayer);

        if (isGrounded)
        {
            //Remember a brief window after leaving ground or platform
            lastGroundTime = coyoteTime;
        }
        else
        {
            lastGroundTime -= Time.deltaTime;
        }

        if(JumpPressedThisFrame)
        {
            //Remember a jump pressed right before landing
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if(lastGroundTime <= 0 || jumpBufferTimer <= 0)
        {
            return; 
        }

        playerCharacter.linearVelocity = new Vector2(playerCharacter.linearVelocity.x, jumpSpeed);

        lastGroundTime = 0;
        jumpBufferTimer = 0;

        jumpedOffLadderTimer = ladderJumpTime;
        AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position, 0.4f);

    }

    private void BetterGravity()

    {

        // Better guard when climbing
        if(playerBodyCollider.IsTouchingLayers(climbingLayer))
        {
            return;
        }

        // Use stronger gravity when falling, then cap fall speed
        float gravityMultiplier = playerCharacter.linearVelocity.y < 0 ? fallGravityMultiplier : 1f;

        playerCharacter.gravityScale = gravityScaleAtStart * gravityMultiplier;

        if(playerCharacter.linearVelocity.y < -jumpSpeed * fallGravityMultiplier)
        {
            playerCharacter.linearVelocity = new Vector2(playerCharacter.linearVelocity.x, -jumpSpeed * fallGravityMultiplier);
        }

    }

   

    private void Climb()

    {
        jumpedOffLadderTimer -= Time.deltaTime;

        if (jumpedOffLadderTimer > 0 || !playerBodyCollider.IsTouchingLayers(climbingLayer))
        {
            playerAnimator.SetBool("climb", false);

            playerCharacter.gravityScale = gravityScaleAtStart;

            // Will stop this function unless true
            return;
        }

        // Support both the Vertical axis and the standard climb keys
        float vMovement = MoveInput.y;

        // Drop horizontal momentum so the Player doesn't glide sideways on the ladder
        Vector2 climbVelocity = new Vector2(MoveInput.x * runSpeed, vMovement * climbSpeed);
        playerCharacter.linearVelocity = climbVelocity;

        playerCharacter.gravityScale = 0.0f;

        bool vSpeed = Mathf.Abs(playerCharacter.linearVelocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("climb", vSpeed);
    }

    private void Die()
    {

        if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        
        {
            isAlive = false;
            
            playerAnimator.SetTrigger("die"); 

            playerCharacter.linearVelocity = deathSeq;

            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.8f);

            FindFirstObjectByType<GameSession>().ProcessPlayerDeath();

        }

    }

    private void WaterGravity()
    
    {
        if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")) || playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            playerCharacter.gravityScale = inWaterGravity; 

        }
       else
        {
            playerCharacter.gravityScale = gravityScaleAtStart;
        }

    }

}
