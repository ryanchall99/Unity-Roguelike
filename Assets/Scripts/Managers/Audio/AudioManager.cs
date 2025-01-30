using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] Sound[] musicSounds, sfxSounds;
    [SerializeField] AudioSource musicSource, sfxSource;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    private void Start() 
    {
        PlayMusic("Theme"); // Always play on start    
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        // If Sound cannot be found within array
        if (s == null)
        {
            Debug.LogError("Music Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("SFX Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
