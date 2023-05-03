using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    public AudioCatalog audioCatalog;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (audioCatalog.backgroundMusic)
        {
            SessionManager.Instance.playMusic(audioCatalog.backgroundMusic);
        }
    }

    public void playEffect(AudioClip effectClip,float volume = 0)
    {
        // stopEffect();
        SessionManager.Instance.playSound(effectClip,volume);
    }

    public void stopEffect()
    {
        SessionManager.Instance.stopSound();
    }
    
}
