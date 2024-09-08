using UnityEngine;


public class Bullet : MonoBehaviour
{

    [Tooltip("The speed at which the bullet travels.")]
    public float speed = 10f;


    [Tooltip("The amount of damage the bullet deals to enemies.")]
    public int damage = 10;


    [Tooltip("The direction in which the bullet is traveling.")]
    private Vector3 travelDirection;

    /// <summary>
    /// Sets the direction for the bullet to travel straight.
    /// </summary>
    /// <param name="direction">The direction for the bullet to travel.</param>
    public void SetDirection(Vector3 direction)
    {
        travelDirection = direction.normalized;  // Normalize to ensure consistent speed
    }

    /// <summary>
    /// Updates the bullet's position every frame.
    /// </summary>
    private void Update()
    {
        // Move bullet in the initially set direction
        transform.Translate(travelDirection * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Handles collisions with other objects.
    /// </summary>
    /// <param name="other">The object that the bullet collided with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy hitEnemy))
        {
            hitEnemy.OnTakeDamage(damage);  // Deal damage to the enemy
            Destroy(gameObject);  // Destroy bullet after hitting
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Boundary"))
        {
            Destroy(gameObject);  // Destroy bullet if it hits the ground or boundary
        }
    }
}