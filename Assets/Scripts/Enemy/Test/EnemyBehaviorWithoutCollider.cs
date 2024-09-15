using UnityEngine;

public class EnemyBehaviorWithoutCollider : MonoBehaviour
{
    public float distanceY = 5f; // The distance for the downward raycast
    public float forceAmount = 5f; // Force to keep the enemy stable
    public Transform centerPosition; // Position to climb to when the front trigger detects another enemy
    public float climbSpeed = 2f; // Speed for climbing to centerPosition
    public bool shouldClimb = false; // Whether the enemy should move to the centerPosition
    public MyCollider2D frontTrigger; // Trigger in the front to detect enemies
    public Vector3 initialPosition; // Initial position of the enemy
    public Rigidbody2D rb; // Initial position of the enemy


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        initialPosition = transform.position; // Save the initial position for stability check
        frontTrigger.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnterFront2D(col));
        frontTrigger.OnTriggerExit2DEvent.AddListener((col) => OnTriggerExitFront2D(col));
    }

    private void Update()
    {
        CastRayAtBottom();

        if (shouldClimb)
        {
            ClimbToCenter();
        }
    }

    // Cast a ray downwards and apply force if not stable
    void CastRayAtBottom()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // Origin slightly above the enemy position
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, distanceY))
        {
            // Apply force to keep stable if not grounded properly
            Vector3 stabilizationForce = (initialPosition - transform.position) * forceAmount;
            rb.AddForce(stabilizationForce, ForceMode2D.Force);
        }
    }

    // Use Slerp to move the enemy towards the center position when triggered
    void ClimbToCenter()
    {
        transform.position = Vector3.Slerp(transform.position, centerPosition.position, Time.deltaTime * climbSpeed);
    }

    // Detect another enemy and start climbing
    private void OnTriggerEnterFront2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Assuming the enemy objects are tagged as "Enemy"
        {
            shouldClimb = true; // Start climbing to the center position
        }
    }

    private void OnTriggerExitFront2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            shouldClimb = false; // Stop climbing when the enemy leaves the trigger
        }
    }
}
