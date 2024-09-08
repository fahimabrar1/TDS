using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("The singleton instance of the game manager.")]
    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        DOTween.Init();
    }
}
