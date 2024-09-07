using UnityEngine;


public abstract class Weapon : MonoBehaviour
{

    [Tooltip("The point from which bullets are fired.")]
    public Transform firePoint;


    [Tooltip("The prefab for the bullet to be fired.")]
    public GameObject bulletPrefab;


    [Tooltip("The renderer for the shooting radius.")]
    public SpriteRenderer shootingRadius;


    [Tooltip("The rate of fire, in seconds between shots.")]
    public float fireRate = 1f;

    /// <summary>
    /// Attacks a specific point.
    /// </summary>
    /// <param name="point">The point to attack.</param>
    public abstract void Attack(Vector3 point);

    /// <summary>
    /// Aims the weapon at a target position.
    /// </summary>
    /// <param name="targetPosition">The position to aim at.</param>
    public virtual void AimAt(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Shows or hides the shooting radius.
    /// </summary>
    /// <param name="value">Whether to show the shooting radius.</param>
    public void ShowShootingRadius(bool value)
    {
        if (shootingRadius.enabled != value)
            shootingRadius.enabled = value;
    }
}