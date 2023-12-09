using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FUISoundMap
{
    public EUISoundType Type;
    public AudioClip Sound;
}

[RequireComponent(typeof(AudioSource))]
public class UISoundManager : MonoBehaviour
{
    /* 컴포넌트 */
    AudioSource audioSource;
    
    /* 필드 */
    [SerializeField] FUISoundMap[] soundMaps;
    Dictionary<EUISoundType, AudioClip> soundDictionary;

    /* MonoBehaviour */
    void Awake()
    {
        // 컴포넌트 할당
        audioSource = GetComponent<AudioSource>();
        
        // 컴포넌트 초기화
        audioSource.playOnAwake = false;
        
        // 캐싱
        soundDictionary = new Dictionary<EUISoundType, AudioClip>(soundMaps.Length);
        foreach (var soundMap in soundMaps)
        {
            RegisterSoundMap(soundMap);
        }
    }
    
    /* API */
    public void PlaySound(EUISoundType soundType)
    {
        if (!soundDictionary.TryGetValue(soundType, out var audioClip))
        {
#if UNITY_EDITOR
            Debug.LogWarning(soundType + " 에 해당하는 소리가 등록되지 않았습니다.");
#endif
            return;
        }

        audioSource.clip = audioClip;
        audioSource.Play();
    }
    
    /* 메서드 */
    void RegisterSoundMap(FUISoundMap soundMap)
    {
        // 유효성 검사
        if (soundMap.Type == EUISoundType.None || !soundMap.Sound)
        {
#if UNITY_EDITOR
            Debug.LogWarning("유효하지 않은 사운드 데이터가 존재합니다.");
#endif
            return;
        }
            
        // 사운드 등록
        soundDictionary.Add(soundMap.Type, soundMap.Sound);
    }
}
