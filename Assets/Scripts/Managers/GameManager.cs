using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("The singleton instance of the game manager.")]
    public static GameManager instance;


    [Tooltip("The Health Data")]
    public HealthData healthData;
    public FileProcessorr<HealthData> healthDataProcessor;

    [Tooltip("The Energy Data")]

    public EnergyGenerateData energyGenerateData;
    public FileProcessorr<EnergyGenerateData> energyGenerateDataProcessor;

    public LevelMenuUI levelMenuUI;


    void Awake()
    {

        levelMenuUI = FindAnyObjectByType<LevelMenuUI>();
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DOTween.Init();

        healthData = new();
        healthDataProcessor = new();

        energyGenerateData = new();
        energyGenerateDataProcessor = new();
        LoadData();
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    async void LoadData()
    {
        await healthDataProcessor.OnLoadAsync(healthData);
        await energyGenerateDataProcessor.OnLoadAsync(energyGenerateData);
        levelMenuUI.InitialzieUI();
    }




    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        healthDataProcessor.OnSaveAsync(healthData);
        energyGenerateDataProcessor.OnSaveAsync(energyGenerateData);
    }
}
