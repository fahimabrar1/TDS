using UnityEngine;
using DG.Tweening;

public class ZombieClimbOnCollider : MonoBehaviour
{
    public float dirX;
    public float moveSpeed = 3f;
    public float jumpForce = 3f;
    public float maxUpwardVelocity = 5f; // The maximum upward velocity cap
    public float speedIncreaseDuration = 1f; // Duration for smooth speed recovery
    public Rigidbody2D rb;

    public MyCollider2D frontCollier;
    private float originalMoveSpeed; // To store the original speed

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        originalMoveSpeed = moveSpeed; // Save the original movement speed

        // Listen to the trigger event on the front collider
        frontCollier.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnterFront2D(col));
        frontCollier.OnTriggerExit2DEvent.AddListener((col) => OnTriggerExitFront2D(col));
    }

    void FixedUpdate()
    {
        // Keep horizontal movement the same, moveSpeed is updated accordingly
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // Cap the upward velocity
        if (rb.velocity.y > maxUpwardVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxUpwardVelocity);
        }
    }

    #region Colliders

    /// <summary>
    /// Detects when an enemy enters the front collider and makes the zombie jump and slows the speed.
    /// </summary>
    public virtual void OnTriggerEnterFront2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                rb.AddForce(Vector2.up * jumpForce); // Apply jump force
                moveSpeed = 0.1f; // Slow down when enemy is in front
                break;
        }
    }

    /// <summary>
    /// Detects when the enemy leaves the front collider and gradually increases the speed.
    /// </summary>
    public virtual void OnTriggerExitFront2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Gradually increase moveSpeed back to the original value
            DOTween.To(() => moveSpeed, x => moveSpeed = x, originalMoveSpeed, speedIncreaseDuration);
        }
    }

    #endregion
}
