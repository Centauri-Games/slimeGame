using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> musica;
    [SerializeField] List<AudioClip> sfx;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource audioSourceLoop;
    [SerializeField] AudioSource audioSource;


    static AudioManager instance;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            musicSource.PlayOneShot(musica[3]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playMusic(int n)
    {
        musicSource.Stop();
        musicSource.PlayOneShot(musica[n]);
    }

    public void playSound(int n, float volume)
    {
        audioSource.PlayOneShot(sfx[n], volume);
    }

    public void playSoundLoop(int n, float volume)
    {
        audioSourceLoop.PlayOneShot(sfx[n], volume);
    }

    public void stopSoundLoop()
    {
        audioSourceLoop.Stop();
    }
}
