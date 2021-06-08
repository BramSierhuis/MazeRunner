using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to play sounds
public class SoundsPlayer : MonoBehaviour
{
    #region Variables
    [Header("Sound Clips")]
    [SerializeField]
    private AudioClip[] coinPickupSounds;
    [SerializeField]
    private AudioClip finalCoinPickup;
    [SerializeField]
    private AudioClip gameOver;
    [SerializeField]
    private AudioClip inGameMusic;
    [SerializeField]
    private AudioClip menuMusic;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource shortEffects; //Source used for short effects
    [SerializeField]
    private AudioSource background; //Source used for long background music
    #endregion

    #region Singleton
    [HideInInspector]
    public static SoundsPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    void Start()
    {
        //When the game starts the menu is always open
        PlayMenuMusic();
    }

    //Speed up the sound, called by GameManager
    public void SetInGamePitch(float pitch)
    {
        background.pitch = pitch;
    }

    //Functions in region are used to play a specific sound
    #region PlayClips
    public void PlayIngameMusic()
    {
        background.pitch = 1;
        background.clip = inGameMusic;
        background.Play();
    }

    public void PlayMenuMusic()
    {
        background.pitch = 1;
        background.clip = menuMusic;
        background.PlayDelayed(1);
    }

    public void PlayFinish()
    {
        shortEffects.clip = finalCoinPickup;
        shortEffects.Play();
    }

    public void PlayGameOver()
    {
        shortEffects.clip = gameOver;
        shortEffects.Play();
    }

    public void PlayCoinPickup()
    {
        shortEffects.clip = coinPickupSounds[Random.Range(0, coinPickupSounds.Length)];
        shortEffects.Play();
    }
    #endregion
}
