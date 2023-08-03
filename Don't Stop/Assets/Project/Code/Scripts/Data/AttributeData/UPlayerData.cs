using System;
using Framework;
using UnityEngine;

[Serializable]
public class UPlayerData : UAttributeData<PlayerDefinition, UBasicAttribute>
{
    // Player Name
    [SerializeField] string m_PlayerName;
    public event Action<string> OnPlayerNameUpdate;
    public string PlayerName
    {
        get => m_PlayerName;
        set
        {
            m_PlayerName = value;
            OnPlayerNameUpdate?.Invoke(value);
        }
    }

    [SerializeField] int m_PlayerID;
    public event Action<int> OnPlayerIDUpdate;

    public int PlayerID
    {
        get => m_PlayerID;
        set
        {
            m_PlayerID = value;
            OnPlayerIDUpdate?.Invoke(value);
        }
    }

    /* Data Model 인터페이스 */
    public override void ManualBroadcast()
    {
        base.ManualBroadcast();
        OnPlayerNameUpdate?.Invoke(m_PlayerName);
        OnPlayerIDUpdate?.Invoke(m_PlayerID);
    }
}
