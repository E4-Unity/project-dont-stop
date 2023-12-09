using UnityEngine;

public class LobbyMenuButton : UISoundButton
{
    /* 필드 */
    [SerializeField] ELobbyMenu menu = ELobbyMenu.None;
    
    /* UISoundButton */
    protected override void OnButtonClick()
    {
        // TODO 버튼 비활성화
        
        // 유효성 검사
        if (!LobbyMenu.TryActivateLobbyMenu(menu)) return;
        
        // 소리 재생
        base.OnButtonClick();
    }
}
