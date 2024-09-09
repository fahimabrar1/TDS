using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Zombie : Enemy
{
    [Tooltip("The speed at which the zombie moves towards the player or crate.")]
    public float moveSpeed = 2f;


    [Tooltip("Time delay between each attack in seconds.")]
    public float attackDelay = 1f;

    [Header("Shake Effect on Attack")]
    [Tooltip("The duration of the shake effect when the zombie attacks.")]
    public float shakeDuration = 0.2f;

    [Tooltip("The strength of the shake in each axis.")]
    public Vector3 shakeStrength = new Vector3(0.3f, 0.3f, 0);

    [Tooltip("The number of shakes during the attack effect.")]
    public int shakeVibrato = 10;

    [Tooltip("The randomness factor for the shake.")]
    public float shakeRandomness = 90f;

    public override void Start()
    {
        Health = 100;
        healthBar.InitializeHealthBar(Health);
        // Find player position from the LevelManager
        target = LevelManager.instance.playerTransform;
    }

    private void Update()
    {
        if (target != null)
        {
            if (targetsInRange.Count == 0 && !isAttacking)
            {
                MoveForward();
            }
        }
    }

    /// <summary>
    /// Moves the zombie forward in the direction it is facing.
    /// </summary>
    private void MoveForward()
    {
        // Move in the direction the zombie is facing
        transform.Translate(moveSpeed * Time.deltaTime * -Vector2.right, Space.Self);
    }

    /// <summary>
    /// Creates a shake effect using DOTween when the zombie attacks.
    /// </summary>
    private void ShakeOnAttack()
    {
        transform.DOShakeScale(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
                .SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// Called when a trigger collider exits the detection zone in front of the zombie.
    /// </summary>
    public override void OnTriggerExit2D(Collider2D other)
    {
        // Stop attacking when the target leaves the detection zone
        isAttacking = false;
    }

    /// <summary>
    /// Coroutine for repeatedly attacking the target with a bounce animation.
    /// </summary>
    /// <param name="damagableTarget">The target to attack.</param>
    public override IEnumerator AttackRoutine(IDamagable damagableTarget)
    {
        isAttacking = true;
        while (target != null && targetsInRange.Count != 0)
        {
            if (damagableTarget != null)
            {
                // Attack and bounce when in range
                OnAttack(damagableTarget);
                ShakeOnAttack();
            }
            // Wait for the attack delay before attacking again
            yield return new WaitForSeconds(attackDelay);
        }
        isAttacking = false;
    }
}
