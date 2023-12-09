using UnityEngine;

public class SurvivalGameState : GameState<SurvivalGameState>
{
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    PlayerEquipment m_EquipmentComponent;
    PlayerStats m_StatsComponent;
    SurvivalEquipmentComponent m_SurvivalEquipmentComponent;

    public PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;
    public PlayerStats GetStatsComponent() => m_StatsComponent;
    public SurvivalEquipmentComponent GetSurvivalEquipmentComponent() => m_SurvivalEquipmentComponent;
    
    /* 필드 */
    [SerializeField] int m_Gold;
    [SerializeField] int m_Exp;

    /* 프로퍼티 */
    public int Gold
    {
        get => m_Gold;
        set => m_Gold = value;
    }

    public int Exp
    {
        get => m_Exp;
        set => m_Exp = value;
    }
    
    /* MonoBehaviour */
    protected override void Awake()
    {
        base.Awake();
        
        // 컴포넌트 할당 및 초기화
        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();
        
        m_StatsComponent = GetComponent<PlayerStats>();
        GetStatsComponent().Init(GetEquipmentComponent());
        
        m_SurvivalEquipmentComponent = GetComponent<SurvivalEquipmentComponent>();
        m_SurvivalEquipmentComponent.Init();
    }

    /* 메서드 */

    protected override void CopyPlayerState()
    {
        var playerEquipment = m_PlayerState.GetEquipmentComponent();

        // Weapon
        var weaponData = playerEquipment.WeaponData;
        UWeaponData weaponCopy = new UWeaponData();
        weaponCopy.Definition = weaponData.Definition;
        weaponCopy.Level = weaponData.Level;
        weaponCopy.Exp = weaponData.Exp;

        m_EquipmentComponent.WeaponData = weaponCopy;

        // Gears
        foreach (var gearSlot in playerEquipment.GearSlots)
        {
            UGearData gearCopy = new UGearData();
            FSavedAttributeData saveData = gearSlot.Value.GetSaveData();
            gearCopy.Init(saveData);
            m_EquipmentComponent.GearSlots.Add(gearSlot.Key, gearCopy);
            m_EquipmentComponent.GearDataList.Add(gearCopy);
        }
    }

    protected override void UpdatePlayerState()
    {
        m_PlayerState.GetInventoryComponent().Gold += Gold;
        m_PlayerState.PlayerData.Exp += Exp / 10;
        m_PlayerState.SaveData();
    }
}
