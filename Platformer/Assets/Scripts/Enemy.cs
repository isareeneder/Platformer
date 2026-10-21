using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private Animator animator;

    Rigidbody2D enemyCharacter;
    Collider2D enemyCollider;

    //[Header("Health")]
    public int maxHealth = 3;
    int currentHealth;

    [HideInInspector] public bool isDead = false;

    void Start()
    {
        enemyCharacter = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; 
        isDead = true;

        Debug.Log("Enemy died!");

        animator.SetTrigger("die");

        enemyCharacter.linearVelocity = Vector2.zero;
        enemyCharacter.bodyType = RigidbodyType2D.Kinematic; 

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        Destroy(gameObject, 1.0f);
    }

    void Update()
    {
        if (isDead) return;

        if (IsFacingRight())
        {
            enemyCharacter.linearVelocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            enemyCharacter.linearVelocity = new Vector2(-moveSpeed, 0);
        }

        if (enemyCharacter.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDead) return;

        transform.localScale = new Vector2(-(Mathf.Sign(enemyCharacter.linearVelocity.x)), 1.0f);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
}
