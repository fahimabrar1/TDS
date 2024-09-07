using UnityEngine;
using UnityEngine.UI;

public class Crate : MonoBehaviour, IDamagable
{
    public int Health { get; set; } = 30;  // Crate health

    public int currentHealth;
    public Image progressBar;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        currentHealth = Health;
        OnUpdateHealth();
    }



    public void OnTakeDamage(int damage)
    {
        Health -= damage;
        OnUpdateHealth();
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


    private void OnUpdateHealth()
    {
        if (currentHealth >= 0)
            progressBar.fillAmount = currentHealth / Health;
    }
}
