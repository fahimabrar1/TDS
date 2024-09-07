using UnityEngine;
using System.Collections;


public class AutomaticRifle : Weapon
{
    [Tooltip("The number of shots in a burst.")]
    public int bulletsPerBurst = 4;

    [Tooltip("The delay between bullets in a burst, in seconds.")]
    public float delayBetweenBullets = 0.1f;

    [Tooltip("The delay between bursts, in seconds.")]
    public float delayBetweenBursts = 2f;

    private bool isShooting = false;

    /// <summary>
    /// Attacks a specific point with the automatic rifle.
    /// </summary>
    /// <param name="point">The point to attack.</param>
    public override void Attack(Vector3 point)
    {
        if (!isShooting)
        {
            StartCoroutine(ShootBurst());
        }
    }

    /// <summary>
    /// Coroutine to shoot a burst of bullets in the firePoint's direction.
    /// </summary>
    private IEnumerator ShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            ShootBulletInFirePointDirection();
            yield return new WaitForSeconds(delayBetweenBullets);
        }

        yield return new WaitForSeconds(delayBetweenBursts);
        isShooting = false;
    }

    /// <summary>
    /// Shoot a bullet in the direction the firePoint is facing.
    /// </summary>
    private void ShootBulletInFirePointDirection()
    {
        // Add 90 degrees to the current firePoint rotation if needed
        Quaternion newRotation = firePoint.rotation * Quaternion.Euler(0, 0, -90);

        // Instantiate the bullet at the firePoint's position and with the adjusted rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, newRotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();

        // Set the bullet's direction to the firePoint's forward direction (or up for 2D)
        bulletComponent.SetDirection(firePoint.right);  // firePoint.right for 2D, firePoint.forward for 3D
    }
}