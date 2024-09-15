using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform target; // The object both enemies want to move towards
    public float moveSpeed = 5f; // Speed of the enemy's movement
    public float jumpForce = 10f; // Force applied to jump over the enemy
    public float jumpDuration = 0.5f; // Duration of the jump
    public float checkRadius = 0.5f; // Radius for collision checking

    private Rigidbody2D rb;
    private bool isJumping = false;
    private float jumpStartTime;
    private Vector2 jumpStartPos;
    private Vector2 jumpEndPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        if (!isJumping)
        {
            // Move towards the target
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            // Check if there's an enemy in front
            CheckForEnemyAndJump();
        }
        else
        {
            // Handle smooth jumping over the enemy
            HandleJump();
        }
    }

    private void CheckForEnemyAndJump()
    {
        // Use a circle overlap to detect enemies in front
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);

        bool foundEnemyInFront = false;
        bool enemyInBack = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.CompareTag("Enemy"))
            {
                Vector2 enemyDirection = (hitCollider.transform.position - transform.position).normalized;

                if (Vector2.Dot(enemyDirection, rb.velocity.normalized) > 0) // Enemy in front
                {
                    foundEnemyInFront = true;
                }
                else // Enemy in back
                {
                    enemyInBack = true;
                }
            }
        }

        if (foundEnemyInFront && !enemyInBack)
        {
            StartJumpOverEnemy();
        }
    }

    private void StartJumpOverEnemy()
    {
        isJumping = true;
        jumpStartTime = Time.time;
        jumpStartPos = rb.position;

        // Calculate jump end position (assuming a simple vertical jump)
        jumpEndPos = new Vector2(transform.position.x, transform.position.y + 2f); // Adjust height as needed
    }

    private void HandleJump()
    {
        float t = (Time.time - jumpStartTime) / jumpDuration;
        if (t > 1f) t = 1f;

        Vector2 currentPos = Vector2.Lerp(jumpStartPos, jumpEndPos, t);
        rb.position = currentPos;

        if (t >= 1f)
        {
            isJumping = false;
        }
    }
}
