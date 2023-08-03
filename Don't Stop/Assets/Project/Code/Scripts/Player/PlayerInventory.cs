using System;
using UnityEngine;

[Serializable]
public class PlayerInventory : MonoBehaviour, IDataModel
{
    [SerializeField] int m_Gold;
    [SerializeField] int m_Crystal;

    public event Action<int> OnGoldUpdate;
    public event Action<int> OnCrystalUpdate;

    public int Gold
    {
        get => m_Gold;
        set
        {
            m_Gold = value;
            OnGoldUpdate?.Invoke(value);
        }
    }

    public int Crystal
    {
        get => m_Crystal;
        set
        {
            m_Crystal = value;
            OnCrystalUpdate?.Invoke(value);
        }
    }

    public void ManualBroadcast()
    {
        OnGoldUpdate?.Invoke(m_Gold);
        OnCrystalUpdate?.Invoke(m_Crystal);
    }
}
