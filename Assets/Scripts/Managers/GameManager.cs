using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("The singleton instance of the game manager.")]
    public static GameManager instance;


    public HealthData healthData;
    public FileProcessorr<HealthData> healthDataProcessor = new();




    void Awake()
    {

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DOTween.Init();
        healthData = new();
        LoadData();
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    async void LoadData()
    {
        await healthDataProcessor.OnLoadAsync(healthData);
    }




    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        healthDataProcessor.OnSaveAsync(healthData);
    }
}
