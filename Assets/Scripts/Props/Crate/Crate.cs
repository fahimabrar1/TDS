using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Crate : MonoBehaviour, IPlayerDamagable
{
    public int Health { get; set; } = 30;  // Crate health

    [Header("Health")]

    public HealthBar healthBar;
    public Image healthProgressBar;

    public Tween healthTween;

    private bool showHealthBar = false;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public virtual void Start()
    {
        healthBar.InitializeHealthBar(Health);
    }


    public virtual void OnTakeDamage(int damage)
    {
        healthBar.DeduceHealth(damage);
        healthBar.ShowHealthbar();
        MyDebug.Log($"Zombie took {damage} damage. Health remaining: {healthBar.currentHealth}");

        if (healthBar.currentHealth <= 0)
        {
            DestroyCrate();
        }
        else
        {
            healthBar.UpdateHealthbar();
        }
    }

    private void DestroyCrate()
    {
        MyDebug.Log("Crate has been destroyed!");
        // Handle crate destruction (e.g., play destruction animation, drop items)
        Destroy(gameObject);
    }

}
