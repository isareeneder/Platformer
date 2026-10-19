using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;

    [SerializeField] private Animator animator;

    Rigidbody2D enemyCharacter;

    //[Header("Health")]
    public int maxHealth = 3;
    int currentHealth;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCharacter = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;



        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetTrigger("die");

        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(IsFacingRight())
        {
        enemyCharacter.linearVelocity = new Vector2(moveSpeed, 0);
        }

        else
        {
        enemyCharacter.linearVelocity = new Vector2(-moveSpeed, 0);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyCharacter.linearVelocity.x)), 1.0f);

    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

}
