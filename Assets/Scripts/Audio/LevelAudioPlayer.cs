using System.Collections.Generic;
using UnityEngine;

public class LevelAudioPlayer : MonoBehaviour
{
    public AudioManager audioManager;

    public List<AudioModel> AllAudios;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

        for (int i = 0; i < AllAudios.Count; i++)
        {
            AllAudios[i].audioSource = gameObject.AddComponent<AudioSource>();
            AllAudios[i].InitializeAudioSource();
        }

    }


    public void OnPlayAudioByName(string name)
    {
        MyDebug.Log($"On {name} audio played");
        if (PlayerPrefs.GetInt(GameConstants.SoundOnName) == 0)
            return;
        var foundModel = AllAudios.Find((model) => model.name.Equals(name));
        foundModel.Play();
    }


    public void OnStopAudioByName(string name)
    {
        MyDebug.Log("On Token Move audio played");
        if (PlayerPrefs.GetInt(GameConstants.SoundOnName) == 0)
            return;
        var foundModel = AllAudios.Find((model) => model.name.Equals(name));
        foundModel.Stop();
    }



    public void OnMusicCheck()
    {
        if (audioManager == null)
            audioManager = FindAnyObjectByType<AudioManager>();

        audioManager.MusicSettingsCheck();
    }

}
