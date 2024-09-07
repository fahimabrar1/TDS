using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IAttackable
{
    [Tooltip("The initial health of the enemy.")]
    public int Health { get; set; } = 50;  // Initial zombie health

    public virtual void OnTakeDamage(int damage)
    {
        Health -= damage;
        MyDebug.Log($"Zombie took {damage} damage. Health remaining: {Health}");

        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void OnAttack(IDamagable target)
    {
        int attackDamage = 15;  // Example damage value
        target.OnTakeDamage(attackDamage);
        MyDebug.Log("Zombie attacked!");
    }

    private void Die()
    {
        MyDebug.Log("Zombie has died!");
        // Handle zombie death (e.g., despawn, play death animation)
        Destroy(gameObject);  // Destroy the zombie when dead
    }
}