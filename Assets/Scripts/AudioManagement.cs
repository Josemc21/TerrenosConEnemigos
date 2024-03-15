using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    // Start is called before the first frame update
    public AudioClip background;
    public AudioClip gun;
    public AudioClip hurtPlayer;
    public AudioClip hurtEnemy;
    public AudioClip deathPlayer;
    public AudioClip deathEnemy;

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
