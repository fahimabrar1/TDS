using UnityEngine;
using DG.Tweening;

public class ZombieClimbWithDOTween : MonoBehaviour
{
    public float detectionDistance = 0.2f; // Distance to check for another zombie in front
    public Transform centerPosition;        // Target position to jump to
    public float climbDuration = 1f;        // Duration of the climb animation

    private bool isClimbing = false;

    void Update()
    {
        if (!isClimbing)
        {
            CheckForZombie();
        }
    }

    void CheckForZombie()
    {
        // Perform a raycast to detect zombies in front of the current zombie
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, detectionDistance);

        // Draw a line representing the detection ray (for debugging purposes)
        Debug.DrawRay(transform.position, Vector2.left * detectionDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // If another zombie is detected, start the jump path animation
            isClimbing = true;
            StartJumpPath();
        }
    }

    void StartJumpPath()
    {
        // Calculate the jump path from current position to the center position
        Vector3[] path = { transform.position, centerPosition.position };

        // Use DOTween to animate the jump path
        transform.DOMove(centerPosition.position, climbDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => isClimbing = false); // Set climbing to false when animation completes

        // Optionally, you can add a vertical movement or a jump effect to make it more realistic
        transform.DOLocalMoveY(centerPosition.position.y + 1f, climbDuration / 2)
            .SetRelative()
            .SetEase(Ease.OutCubic)
            .OnComplete(() => transform.DOLocalMoveY(centerPosition.position.y, climbDuration / 2).SetEase(Ease.InCubic));
    }

    void OnDrawGizmos()
    {
        // Draw a gizmo to visualize the detection distance
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * detectionDistance);

        // Draw a gizmo to visualize the center position
        if (centerPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, centerPosition.position);
        }
    }
}
