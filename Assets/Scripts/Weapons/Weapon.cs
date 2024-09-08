using UnityEngine;


public abstract class Weapon : MonoBehaviour
{

    [Tooltip("The point from which bullets are fired.")]
    public Transform firePoint;


    [Tooltip("The prefab for the bullet to be fired.")]
    public GameObject bulletPrefab;


    [Tooltip("The prefab for the powerred bullet to be fired.")]
    public GameObject poweredBulletPrefab;


    [Tooltip("The renderer for the shooting radius.")]
    public SpriteRenderer shootingRadius;


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



    /// <summary>
    /// Enables powered mode for a set duration.
    /// </summary>
    public virtual void ActivatePoweredMode()
    {
    }
}