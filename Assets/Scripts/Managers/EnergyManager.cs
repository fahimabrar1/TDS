using System;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;
    public Action<int> OnUpdateButtonsAction;

    public EnerygyGenerator enerygyGenerator;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}