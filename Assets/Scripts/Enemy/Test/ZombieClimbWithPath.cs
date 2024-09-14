using UnityEngine;
using DG.Tweening;

public class ZombieClimbWithPath : MonoBehaviour
{
    public float detectionDistance = 0.2f; // Distance to check for another zombie in front
    public Transform centerPosition;        // Target position to jump to
    public float climbDuration = 1f;        // Duration of the climb animation

    public bool isClimbing = false;

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



        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // Draw a line representing the detection ray (for debugging purposes)
            Debug.DrawLine(transform.position, hit.point, Color.red);

            // If another zombie is detected, start the jump path animation
            isClimbing = true;
            StartJumpPath(centerPosition.position);
        }
    }

    void StartJumpPath(Vector3 targetPosition)
    {
        // Create a path with a curve
        Vector3[] path = { transform.position, new Vector3(transform.position.x, targetPosition.y, transform.position.z), targetPosition };

        // Use DOTween to animate the path
        transform.DOPath(path, climbDuration, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => isClimbing = false); // Set climbing to false when animation completes
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
