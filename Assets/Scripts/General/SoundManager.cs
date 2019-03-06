using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    [Header("Player's SFX")]
    public AudioClip playerShoot;

    [Header("In-Game SFX")]
    public AudioClip chestOpen;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        sfxSource.volume = 0.5f;
        musicSource.volume = 0.5f;
    }

    public void playSingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void randomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }

    //! Player's SFX Functions

    public void pShoot()
    {
        sfxSource.PlayOneShot(playerShoot);
    }

    //! In-Game SFX Functions

    public void cInteract()
    {
        sfxSource.PlayOneShot(chestOpen);
    }

    //! Enemies's SFX Functions
}