using System;
using System.Collections.Generic;
using UnityEngine;

internal enum ELobbyMenu
{
    None,
    Shop, // 상점
    Equipment, // 장비
    Combat, // 전투
    Bunker, // 벙커
    Enhancement // 강화
}

[Serializable]
internal struct FLobbyMenu
{
    [SerializeField] internal ELobbyMenu Menu;
    [SerializeField] internal GameObject Panel;
}

internal class LobbyMenu : MonoBehaviour
{
    /* Static */
    static Dictionary<ELobbyMenu, GameObject> LobbyMenuDictionary;
    static ELobbyMenu LastLobbyMenu = ELobbyMenu.None;
    static bool IsInitialized => LobbyMenuDictionary is not null;

    internal static bool TryActivateLobbyMenu(ELobbyMenu selectedMenu)
    {
        // 유효성 검사 및 중복 호출 방지
        if (selectedMenu == ELobbyMenu.None || selectedMenu == LastLobbyMenu) return false;
        
        // 이전 메뉴 비활성화
        if (LobbyMenuDictionary.TryGetValue(LastLobbyMenu, out var lastPanel))
        {
            lastPanel.SetActive(false);
        }
        
        // 새로운 메뉴 활성화
        if (LobbyMenuDictionary.TryGetValue(selectedMenu, out var newPanel))
        {
            newPanel.SetActive(true);
            LastLobbyMenu = selectedMenu; // 선택된 메뉴 캐싱
        }

        return true;
    }
    
    /* 필드 */
    [Header("설정")]
    [SerializeField] FLobbyMenu[] lobbyMenuList; // 캐싱할 메뉴 패널 목록
    [SerializeField] ELobbyMenu defaultMenu = ELobbyMenu.Equipment; // 기본 메뉴 설정

    /* MonoBehaviour */
    void Awake()
    {
        // 중복 호출 방지
        if (IsInitialized) return;
        
        // 캐싱
        LobbyMenuDictionary = new Dictionary<ELobbyMenu, GameObject>(lobbyMenuList.Length);
        foreach (var lobbyMenu in lobbyMenuList)
        {
            RegisterLobbyMenu(lobbyMenu); // 캐싱
            lobbyMenu.Panel.SetActive(false); // 전부 비활성화
        }
        
        // 기본 메뉴 활성화
        TryActivateLobbyMenu(defaultMenu);
    }

    void OnDestroy()
    {
        // 초기화
        LobbyMenuDictionary = null;
        LastLobbyMenu = ELobbyMenu.None;
    }

    /* 메서드 */
    void RegisterLobbyMenu(FLobbyMenu lobbyMenu)
    {
        // 유효성 검사
        if (lobbyMenu.Menu == ELobbyMenu.None || !lobbyMenu.Panel)
        {
#if UNITY_EDITOR
            Debug.LogWarning("유효하지 않은 로비 메뉴 데이터입니다 : " + lobbyMenu.Menu);
#endif
            return;
        }
        
        // 중복 데이터 검사
        if (LobbyMenuDictionary.ContainsKey(lobbyMenu.Menu))
        {
#if UNITY_EDITOR
            Debug.LogWarning("이미 등록된 데이터입니다 : " + lobbyMenu.Menu);
#endif
            return;
        }
        
        // 메뉴 등록
        LobbyMenuDictionary.Add(lobbyMenu.Menu, lobbyMenu.Panel);
    }
}
