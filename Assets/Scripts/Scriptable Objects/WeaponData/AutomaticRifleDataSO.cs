using UnityEngine;

[CreateAssetMenu(fileName = "AutomaticRifleDataS0", menuName = "TDS/Weapon/AutomaticRifleDataS0", order = 0)]
public class AutomaticRifleDataS0 : ScriptableObject
{
    [Header("Basic")]
    [Tooltip("The number of shots in a burst.")]
    public int bulletsPerBurst = 4;

    [Tooltip("The delay between bullets in a burst, in seconds.")]
    public float delayBetweenBullets = 0.1f;

    [Tooltip("The delay between bursts, in seconds.")]
    public float delayBetweenBursts = 2f;


    [Header("Powered")]
    [Tooltip("The number of powered shots in a burst.")]
    public int poweredBulletsPerBurst = 1;

    [Tooltip("The delay between powered bullets in a burst, in seconds.")]
    public float poweredDelayBetweenBullets = 0.1f;

    [Tooltip("The delay between powered bursts, in seconds.")]
    public float poweredDelayBetweenBursts = 0.5f;

    [Tooltip("Duration of the powered mode in seconds.")]
    public float poweredModeDuration = 5f;


    [Header("Audio Names")]


    [Tooltip("The name of the shooting audio.")]
    public string shootingAudio;

    [Tooltip("The name of the heavy shooting audio.")]
    public string shootingHeavyAudio;


}