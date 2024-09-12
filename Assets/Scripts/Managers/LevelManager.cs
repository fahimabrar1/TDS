using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Tooltip("The player in the level.")]
    public Transform playerTransform;

    [Tooltip("The list of crates in the level.")]
    public List<Transform> crates = new();



    private void Awake()
    {
        // Singleton pattern to ensure only one instance of LevelManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void RemoveCrate(Transform transform)
    {
        crates.Remove(transform);
    }
}
