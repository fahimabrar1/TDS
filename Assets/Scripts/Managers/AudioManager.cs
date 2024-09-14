using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioClip musicClip;

    public bool soundOn = true;
    public bool musicOn = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance.MusicSettingsCheck();
            Destroy(gameObject);
        }

        musicSource = GetComponent<AudioSource>();
    }



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

        MusicSettingsCheck();
    }



    public void MusicSettingsCheck()
    {

        // Load sound and music preferences
        soundOn = PlayerPrefs.GetInt(GameConstants.SoundOnName, 1) == 1; // Default is on
        musicOn = PlayerPrefs.GetInt(GameConstants.MusicOnName, 1) == 1; // Default is on
        MyDebug.Log("Calling Setting Check: " + musicOn);
        if (musicOn)
        {
            PlayMusic(musicClip);
        }
        else
        {
            StopMusic();
        }
    }


    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
        PlayerPrefs.SetInt("MusicOn", 1);
        musicOn = true;
    }

    public void StopMusic()
    {
        PlayerPrefs.SetInt("MusicOn", 0);
        musicOn = false;
        musicSource.Stop();
    }
}
