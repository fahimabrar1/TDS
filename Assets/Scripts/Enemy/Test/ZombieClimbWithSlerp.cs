using UnityEngine;

public class ZombieClimbWithSlerp : MonoBehaviour
{
    public float climbDuration = 1f;          // Duration of the climb animation
    public float moveSpeed = 2f;              // Forward movement speed
    public Transform centerPosition;          // Target position to jump to (e.g., another zombie's center)

    public bool isClimbing = false;
    public float climbProgress = 0f;         // Progress of the slerp (0 to 1)
    public Vector3 startClimbPosition;       // Start position of the climb
    public Vector3 targetClimbPosition;      // End position of the climb
    public Vector3 midClimbPosition;         // Midpoint for arc-like movement

    public MyCollider2D frontCollider;
    public MyCollider2D backCollider;
    public MyCollider2D bottomCollider;
    public CapsuleCollider2D capsuleTrigger;
    public CapsuleCollider2D capsuleSolid;
    private Collider2D targetEnemyCollider;  // Reference to the enemy collider

    public float arcHeightFactor = 0.25f;     // Factor to control the height of the arc

    void Start()
    {
        // Ignore collisions between the zombie's own colliders and the capsule
        Physics2D.IgnoreCollision(frontCollider.collider2D, capsuleTrigger, true);
        Physics2D.IgnoreCollision(backCollider.collider2D, capsuleTrigger, true);
        Physics2D.IgnoreCollision(bottomCollider.collider2D, capsuleTrigger, true);

        Physics2D.IgnoreCollision(frontCollider.collider2D, capsuleSolid, true);
        Physics2D.IgnoreCollision(backCollider.collider2D, capsuleSolid, true);
        Physics2D.IgnoreCollision(bottomCollider.collider2D, capsuleSolid, true);

        // Add listener for front collider trigger
        frontCollider.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnterFront2D(col));
    }

    void Update()
    {
        if (!isClimbing)
        {
            MoveForward();  // Keep moving forward when not climbing
        }
        else
        {
            PerformClimb(); // Handle climbing
        }
    }

    // Moves the zombie forward when not climbing
    void MoveForward()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector2.left);
    }

    // Perform the climbing operation when a trigger is hit
    void PerformClimb()
    {
        // Increment climb progress
        climbProgress += Time.deltaTime / climbDuration;

        // Slerp between the start, mid, and target positions to create an arc
        Vector3 positionA = Vector3.Slerp(startClimbPosition, midClimbPosition, climbProgress);
        Vector3 positionB = Vector3.Slerp(midClimbPosition, targetClimbPosition, climbProgress);
        transform.position = Vector3.Slerp(positionA, positionB, climbProgress);

        // Check if the climb is complete
        if (climbProgress >= 1f)
        {
            isClimbing = false;
            climbProgress = 0f; // Reset for future climbs

            // Re-enable collision with the target enemy
            if (targetEnemyCollider != null)
            {
                Physics2D.IgnoreCollision(capsuleSolid, targetEnemyCollider, false);
            }
        }
    }

    // Trigger detection for the front collider
    public virtual void OnTriggerEnterFront2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":

                // Start climbing if an enemy is detected
                isClimbing = true;
                startClimbPosition = transform.position;  // Current position is the start of the climb

                // Set the target position for the climb (jump to the detected enemy's jump point)
                targetClimbPosition = new(other.transform.position.x, other.transform.position.y + (capsuleSolid.offset.y + capsuleSolid.size.y) / 2, other.transform.position.z);

                // Calculate the midpoint based on start and target positions, with a smaller arc height
                float distance = Vector3.Distance(startClimbPosition, targetClimbPosition);
                midClimbPosition = (startClimbPosition + targetClimbPosition) / 2 + Vector3.up * distance * arcHeightFactor;

                // Temporarily disable collision with the target enemy to avoid bouncing back
                targetEnemyCollider = other;
                Physics2D.IgnoreCollision(capsuleSolid, targetEnemyCollider, true);

                break;
        }
    }

    void OnDrawGizmos()
    {
        // Draw a gizmo to visualize the center position
        if (centerPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, centerPosition.position);
        }
    }

    #region Colliders

    // Place additional collider logic if needed in this region

    #endregion
}
