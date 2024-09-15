using UnityEngine;

[CreateAssetMenu(fileName = "ThrowableGranadeDataSO", menuName = "TDS/Weapons/ThrowableGranadeDataSO", order = 0)]
public class ThrowableGranadeDataSO : ScriptableObject
{

    [Header("Throwable Data")]

    [Tooltip("The prefab for the grenade that the player can throw.")]
    public GameObject throwablePrefab;

    [Tooltip("The force applied when the grenade is thrown.")]
    public float throwableThrowForce = 4f;
    [Tooltip("The force applied when the grenade is thrown to spin.")]
    public float throwableSpinForce = 0.2f;


}