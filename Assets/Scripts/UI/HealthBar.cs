using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int iniitalHealth;
    public int currentHealth;

    [Header("Health")]
    public Image healthProgressBar;

    public Tween healthTween;

    private bool showHealthBar = false;



    public void InitializeHealthBar(int iniitalHealth)
    {
        healthProgressBar.fillAmount = 1;
        currentHealth = this.iniitalHealth = iniitalHealth;
        gameObject.SetActive(showHealthBar);
    }


    public void UpdateHealthbar()
    {
        float newFillAmount = currentHealth / (float)iniitalHealth;

        // Use DOTween to animate the fill amount over time
        healthTween = healthProgressBar.DOFillAmount(newFillAmount, 0.2f);
    }


    public void ShowHealthbar()
    {
        if (!gameObject.activeInHierarchy && showHealthBar)
            gameObject.SetActive(showHealthBar);
    }


    public void KillHealthTween()
    {
        healthTween?.Kill();
    }


    public void DeduceHealth(int damage)
    {
        currentHealth -= damage;
        showHealthBar = true;
    }
}
