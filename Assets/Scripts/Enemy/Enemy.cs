using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour, IDamagable, IAttackable
{
    [Tooltip("The initial health of the enemy.")]
    public int Health { get; set; } = 50;  // Initial zombie health
    protected int currentHealth;
    [Header("Health")]
    public GameObject HealthBar;
    public Image healthProgressBar;

    public Tween healthTween;

    private bool showHealthBar = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public virtual void Start()
    {
        healthProgressBar.fillAmount = 1;
        HealthBar.SetActive(showHealthBar);
    }



    public virtual void OnTakeDamage(int damage)
    {
        currentHealth -= damage;
        showHealthBar = true;
        ShowHealthbar();
        MyDebug.Log($"Zombie took {damage} damage. Health remaining: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthbar();
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
        if (healthTween != null)
            healthTween.Kill();
        Destroy(gameObject);  // Destroy the zombie when dead
    }


    public void ShowHealthbar()
    {
        if (!HealthBar.activeInHierarchy && showHealthBar)
            HealthBar.SetActive(showHealthBar);
    }


    public void UpdateHealthbar()
    {
        float newFillAmount = currentHealth / (float)Health;

        // Use DOTween to animate the fill amount over time
        healthTween = healthProgressBar.DOFillAmount(newFillAmount, 0.2f);
    }
}