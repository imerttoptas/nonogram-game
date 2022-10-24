using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] AudioClip[] soundEffects;
    public AudioSource[] audioSources;
    private Dictionary<SoundEffectType, AudioClip> soundEffectDictionary;
    

    private void Start()
    {
        soundEffectDictionary = new Dictionary<SoundEffectType, AudioClip>();
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffectDictionary.Add((SoundEffectType)i, soundEffects[i]);
        }
        SetInitialAudios();
    }
    
    public void PlayEndPanelSound(GameState state)
    {
        if (state == GameState.Win)
        {
            audioSources[1].PlayOneShot(soundEffectDictionary[SoundEffectType.WinSound]);
        }
        else if (state == GameState.Lose)
        {
            audioSources[1].PlayOneShot(soundEffectDictionary[SoundEffectType.LoseSound]);
        }
    }

    public void PlaySoundEffect(SoundEffectType soundEffectType, float volumeLevel = 1f)
    {
        audioSources[1].volume = volumeLevel;
        audioSources[1].PlayOneShot(soundEffectDictionary[soundEffectType]);
    }

    void SetInitialAudios()
    {
        if (LevelManager.instance.UserData.isMusicOn)
        {
            audioSources[0].Play();
        }

        audioSources[1].enabled = LevelManager.instance.UserData.isSoundOn;
    }
}
