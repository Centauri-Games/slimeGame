using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> musica;
    [SerializeField] List<AudioClip> sfx;
    static AudioSource audioSource;

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
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this);
            audioSource.PlayOneShot(musica[3]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playMusic(int n)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(musica[n]);
    }
}
