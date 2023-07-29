using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public enum Sfx
    {
        Dead,
        Hit,
        LevelUp=3,
        Lose,
        Melee,
        Range=7,
        Select,
        Win
    }
        
    static AudioManager Instance;
    public static AudioManager Get() => Instance;

    [Header("[Reference]")]
    [SerializeField] AudioSource m_BgmPlayer;
    [SerializeField] AudioSource[] m_SfxPlayers;
    [SerializeField] AudioHighPassFilter m_BgmEffect;
    
    [Header("[Initialization]")]
    [SerializeField] AudioClip m_BgmClip;
    [SerializeField] float m_BgmVolume;
    [SerializeField] AudioClip[] m_SfxClips;
    [SerializeField] float m_SfxVolume;
    [SerializeField] int m_Channels;
    [SerializeField] int m_ChannelIndex; // 가장 마지막에 플레이한 채널

    /* 메서드 */
    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        m_BgmPlayer = bgmObject.AddComponent<AudioSource>();
        m_BgmPlayer.playOnAwake = false;
        m_BgmPlayer.loop = true;
        m_BgmPlayer.volume = m_BgmVolume;
        m_BgmPlayer.clip = m_BgmClip;
        m_BgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        m_SfxPlayers = new AudioSource[m_Channels];

        for (int i = 0; i < m_SfxPlayers.Length; i++)
        {
            m_SfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            m_SfxPlayers[i].playOnAwake = false;
            m_SfxPlayers[i].bypassListenerEffects = true;
            m_SfxPlayers[i].volume = m_SfxVolume;
        }
    }
    
    /* API */
    public void PlaySfx(Sfx _sfx)
    {
        for (int i = 0; i < m_SfxPlayers.Length; i++)
        {
            int loopIndex = (i + m_ChannelIndex) % m_SfxPlayers.Length;

            if (m_SfxPlayers[loopIndex].isPlaying)
                continue;

            int randIndex = 0;
            if (_sfx == Sfx.Hit || _sfx == Sfx.Melee)
            {
                randIndex = Random.Range(0, 2);
            }

            m_ChannelIndex = loopIndex;
            m_SfxPlayers[loopIndex].clip = m_SfxClips[(int)_sfx + randIndex];
            m_SfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBgm(bool _isPlay)
    {
        if (_isPlay)
        {
            m_BgmPlayer.Play();
        }
        else
        {
            m_BgmPlayer.Stop();
        }
    }
    
    public void EffectBgm(bool _isPlay)
    {
        m_BgmEffect.enabled = _isPlay;
    }
    
    /* MonoBehaviour */
    void Awake()
    {
        Instance = this;
        Init();
    }
}
