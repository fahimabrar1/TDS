using System.Collections;
using UnityEngine;

public class ZombieClimb : MonoBehaviour
{
    public float climbSpeed = 2f;          // Speed at which the zombie climbs over the other zombie
    public float moveSpeed = 2f;           // Speed of zombie moving forward
    public float detectionDistance = 0.5f; // Distance to check for another zombie in front
    public float jumpHeight = 0.5f; // Distance to check for another zombie in front
    [SerializeField]
    private bool isClimbing = false;       // Flag to check if zombie is in climbing state

    // Gizmo properties
    public Color gizmoColor = Color.green; // Color for the detection range
    public Color climbTargetColor = Color.blue; // Color for the climb target

    private Vector2 originalPosition;       // Store original position for Lerp
    public MyCollider2D frontCollider;

    void Start()
    {
        frontCollider.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnterFront2D(col));
        frontCollider.OnTriggerExit2DEvent.AddListener(OnTriggerExitFront2D);
    }

    void Update()
    {
        if (!isClimbing)
        {
            // Move zombie forward continuously
            MoveForward();
        }
    }

    void MoveForward()
    {
        // Move the zombie forward by directly modifying its position
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    IEnumerator ClimbOverZombie(GameObject otherZombie)
    {
        isClimbing = true;
        float climbTargetY = otherZombie.transform.position.y + jumpHeight; // Target position to climb over
        Vector2 targetPosition = new(transform.position.x, climbTargetY);

        while (transform.position.y < climbTargetY) // Continue climbing until we reach the target height
        {
            // Lerp towards the target position for smooth climbing
            transform.position = Vector2.Lerp(transform.position, targetPosition, climbSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame before continuing
        }

        isClimbing = false; // Climbing is complete, resume forward movement
    }

    // This method is used to draw gizmos in the Scene view
    void OnDrawGizmos()
    {
        // Set the color for the detection distance
        Gizmos.color = gizmoColor;

        // Draw the detection ray
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * detectionDistance);

        // If in climbing mode, draw a line to show the target height for the climb
        if (isClimbing)
        {
            Gizmos.color = climbTargetColor;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z));
        }
    }

    #region Colliders

    /// <summary>
    /// Detects when a player enters the attack zone and initiates the climb over the closest one.
    /// </summary>
    public virtual void OnTriggerEnterFront2D(Collider2D other)
    {
        MyDebug.Log($"Entered {other.name}");

        if (other.gameObject.CompareTag("Enemy") && !isClimbing)
        {
            // If another zombie is detected, start climbing
            StartCoroutine(ClimbOverZombie(other.gameObject)); // Start coroutine to handle climbing
        }
    }

    /// <summary>
    /// Called when a trigger collider exits the detection zone.
    /// </summary>
    public virtual void OnTriggerExitFront2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isClimbing = false;
        }
    }

    #endregion
}
