using UnityEngine;

public class EnemyDataSO : ScriptableObject
{

    [Header("Health Data")]
    [Tooltip("The initial health of the enemy.")]
    public int Health = 50;  // Initial zombie health


    [Header("Attack Data")]
    public float attackDelay = 1f;
    public int attackDamage = 50;


}