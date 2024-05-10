using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiManager : MonoBehaviour
{   
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }

    void PlayRandomMusic()
    {
        int randomIndex = Random.Range(0, audioClips.Length + 1);
        AudioClip clipToPlay = audioClips[randomIndex];
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}
