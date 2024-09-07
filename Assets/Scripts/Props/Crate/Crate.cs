using UnityEngine;

public class Crate : MonoBehaviour, IDamagable
{
    public int Health { get; set; } = 30;  // Crate health

    public void OnTakeDamage(int damage)
    {
        Health -= damage;
        MyDebug.Log($"Crate took {damage} damage. Health remaining: {Health}");

        if (Health <= 0)
        {
            DestroyCrate();
        }
    }

    private void DestroyCrate()
    {
        MyDebug.Log("Crate has been destroyed!");
        // Handle crate destruction (e.g., play destruction animation, drop items)
        Destroy(gameObject);
    }
}
