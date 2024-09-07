using UnityEngine;

public class Zombie : Enemy
{

    public override void OnTakeDamage(int damage)
    {
        Health -= damage;
        MyDebug.Log($"Zombie took {damage} damage. Health remaining: {Health}");

        if (Health <= 0)
        {
            Die();
        }
    }

    public override void OnAttack(IDamagable target)
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
