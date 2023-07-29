using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "Scriptable Object / Enemy Data"), Serializable]
public class EnemyData : SerializedScriptableObject
{
    [SerializeField] SpriteLibraryAsset m_SpriteLibraryAsset;
    public SpriteLibraryAsset GetSpriteLibraryAsset() => m_SpriteLibraryAsset;

    [SerializeField] float m_Speed;
    public float Speed => m_Speed;

    [SerializeField] int m_MaxHealth;
    public int MaxHealth => m_MaxHealth;
}
