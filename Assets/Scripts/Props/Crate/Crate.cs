using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Crate : MonoBehaviour, IDamagable
{
    public int Health { get; set; } = 30;  // Crate health

    public int currentHealth; [Header("Health")]
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
        currentHealth = Health;

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
            DestroyCrate();
        }
        else
        {
            UpdateHealthbar();
        }
    }

    private void DestroyCrate()
    {
        MyDebug.Log("Crate has been destroyed!");
        // Handle crate destruction (e.g., play destruction animation, drop items)
        Destroy(gameObject);
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
