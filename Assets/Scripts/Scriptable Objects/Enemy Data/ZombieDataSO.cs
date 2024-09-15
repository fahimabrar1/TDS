using UnityEngine;

[CreateAssetMenu(fileName = "ZombieDataSO", menuName = "TDS/Enemies/ZombieDataSO", order = 0)]
public class ZombieDataSO : EnemyDataSO
{

    [Header("Move Data")]
    public float forwardSpeed = 2f; // Initial movement speed
    public float backwardSpeed = .2f; // Initial movement speed
    public float stopDistance = 0.18f; // Distance to completely stop
    public float brakingFactor = 0.5f; // Speed reduction factor when braking
    public float stopThreshold = 0.1f; // Minimum speed threshold for stopping
    public float acceleration = 2f; // Acceleration rate
    public float raycastDistance = 1f; // How far to check for other enemies



    [Header("Shake Effect on Attack")]
    public float shakeDuration = 0.2f;
    public Vector3 shakeStrength = new(0.3f, 0.3f, 0);
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
}