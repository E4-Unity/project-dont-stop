using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAudioManager : GenericMonoSingleton<SystemAudioManager>
{
    AudioSource m_AudioSource;

    protected override void Awake()
    {
        base.Awake();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlaySelectButton()
    {
        m_AudioSource.Play();
    }
}
