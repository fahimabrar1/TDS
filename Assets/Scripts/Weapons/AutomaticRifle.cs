using UnityEngine;
using System.Collections;

public class AutomaticRifle : Weapon
{

    public AutomaticRifleDataS0 automaticRifleDataS0;

    private bool isShooting = false;
    private bool isPoweredMode = false; // Tracks whether powered mode is active


    private LevelAudioPlayer levelAudioPlayer;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        levelAudioPlayer = FindObjectOfType<LevelAudioPlayer>();
    }


    /// <summary>
    /// Attacks a specific point with the automatic rifle.
    /// </summary>
    /// <param name="point">The point to attack.</param>
    public override void Attack(Vector3 point)
    {
        if (!isShooting)
        {
            StartCoroutine(isPoweredMode ? PoweredShootBurst() : ShootBurst());
        }
    }

    /// <summary>
    /// Coroutine to shoot a burst of regular bullets.
    /// </summary>
    private IEnumerator ShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < automaticRifleDataS0.bulletsPerBurst; i++)
        {
            levelAudioPlayer.OnPlayAudioByName(automaticRifleDataS0.shootingAudio);
            ShootBulletInFirePointDirection(bulletPrefab);
            yield return new WaitForSeconds(automaticRifleDataS0.delayBetweenBullets);
        }

        yield return new WaitForSeconds(automaticRifleDataS0.delayBetweenBursts);
        isShooting = false;
    }

    /// <summary>
    /// Coroutine to shoot a burst of powered bullets.
    /// </summary>
    private IEnumerator PoweredShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < automaticRifleDataS0.poweredBulletsPerBurst; i++)
        {
            levelAudioPlayer.OnPlayAudioByName(automaticRifleDataS0.shootingHeavyAudio);
            ShootBulletInFirePointDirection(poweredBulletPrefab);
            yield return new WaitForSeconds(automaticRifleDataS0.poweredDelayBetweenBullets);
        }

        yield return new WaitForSeconds(automaticRifleDataS0.poweredDelayBetweenBursts);
        isShooting = false;
    }

    /// <summary>
    /// Shoot a bullet in the direction the firePoint is facing.
    /// </summary>
    /// <param name="bulletPrefab">The prefab for the bullet to shoot (either normal or powered).</param>
    private void ShootBulletInFirePointDirection(GameObject bulletPrefab)
    {
        // Add 90 degrees to the current firePoint rotation if needed
        Quaternion newRotation = firePoint.rotation * Quaternion.Euler(0, 0, -90);

        // Instantiate the bullet at the firePoint's position with the adjusted rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, newRotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();

        // Set the bullet's direction to the firePoint's right direction (or up for 2D)
        bulletComponent.SetDirection(firePoint.right);  // firePoint.right for 2D, firePoint.forward for 3D
    }

    /// <summary>
    /// Enables powered mode for a set duration.
    /// </summary>
    public override void ActivatePoweredMode()
    {
        if (!isPoweredMode)
        {
            StopCoroutine(ShootBurst());

            StartCoroutine(PoweredModeRoutine());
        }
    }

    /// <summary>
    /// Coroutine to manage powered mode duration.
    /// </summary>
    private IEnumerator PoweredModeRoutine()
    {
        isPoweredMode = true;
        MyDebug.Log("Powered mode activated!");
        StartCoroutine(PoweredShootBurst());
        // Wait for the duration of the powered mode
        yield return new WaitForSeconds(automaticRifleDataS0.poweredModeDuration);

        isPoweredMode = false;
        MyDebug.Log("Powered mode ended!");
    }
}
