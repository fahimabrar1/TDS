using DG.Tweening;
using UnityEngine;

public class CustomEnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 2f; // Initial movement speed
    public float detectDistance = 5f; // Distance to detect other enemies
    public float stopDistance = 0.18f; // Distance to completely stop
    public float brakingFactor = 0.5f; // Speed reduction factor when braking
    public float stopThreshold = 0.1f; // Minimum speed threshold for stopping
    public float acceleration = 2f; // Acceleration rate
    public Rigidbody2D rb;
    public MyCollider2D frontCollider;

    public BoxCollider2D boxCollider2D;
    public CapsuleCollider2D capsuleCollider2D;
    private float currentSpeed;

    public bool canMove = true;

    Collider2D ignoreColliders;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        frontCollider.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnter2DFront(col));
        currentSpeed = moveSpeed; // Set current speed to the initial move speed
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // Cast a ray in the left direction to detect enemies
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.left, detectDistance);
            Debug.DrawRay(transform.position, Vector2.left * detectDistance, Color.red);

            bool enemyDetected = false;

            foreach (var hit in hits)
            {
                var hitCollider = hit.collider;
                if (hitCollider != null && hitCollider != boxCollider2D && hitCollider != ignoreColliders && hitCollider.gameObject.CompareTag("Enemy"))
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, hitCollider.transform.position);

                    MyDebug.Log("Detected Enemy: " + hitCollider.name + " at distance: " + distanceToEnemy);
                    enemyDetected = true;

                    // If the enemy is within stop distance, stop movement
                    if ((distanceToEnemy - stopDistance) < .1f)
                    {
                        currentSpeed = 0f; // Stop completely
                        canMove = false; // Disable movement
                        ignoreColliders = hit.collider;
                        Climb(new Vector3(hit.transform.position.x, hit.transform.position.y + (Mathf.Abs(capsuleCollider2D.offset.y) + capsuleCollider2D.size.y / 4) / 2, hit.transform.position.z));
                        break;
                    }
                    else
                    {
                        // If the enemy is within detectDistance but further than stopDistance, start braking
                        float brakeFactor = Mathf.Clamp01((distanceToEnemy - stopDistance) / (detectDistance - stopDistance));
                        currentSpeed = moveSpeed * brakeFactor; // Slow down based on proximity
                    }
                }
            }

            // If no enemy is detected, reset to normal speed
            if (!enemyDetected)
            {
                // Gradually increase speed to the target moveSpeed
                if (currentSpeed < moveSpeed)
                {
                    currentSpeed += acceleration * Time.fixedDeltaTime;
                    if (currentSpeed > moveSpeed)
                    {
                        currentSpeed = moveSpeed;
                    }
                }
                canMove = true;
            }

            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        // Apply the calculated speed to move the enemy left
        rb.velocity = Vector2.left * currentSpeed;
    }

    public void Climb(Vector3 targetPosition)
    {
        // Create a path with a curve
        Vector3[] path = {
            transform.position,
            new Vector3((transform.position.x + targetPosition.x) / 2, transform.position.y + 0.15f, transform.position.z),
            targetPosition
        };

        // Disable physics during the animation to avoid conflicts
        rb.isKinematic = true;
        rb.velocity = Vector2.zero; // Stop any velocity during the animation

        // Use DOTween to animate the path
        transform.DOPath(path, 1, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // After completing the path, reset the position
                transform.position = targetPosition;

                // Re-enable physics and start accelerating
                rb.isKinematic = false;
                rb.velocity = Vector2.zero; // Reset velocity to avoid sudden jumps

                // Set canMove to true to allow movement after the climb
                canMove = true;
            });
    }

    private void OnTriggerEnter2DFront(Collider2D col)
    {
        // Handle the front trigger events here
    }
}
