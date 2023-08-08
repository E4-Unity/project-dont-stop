using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Config/Game", fileName = "Game Config")]
public class GameConfig : ScriptableObject
{
    // 리스트에 추가된 순서대로 매니저 게임 오브젝트를 SetActive(true)
    [SerializeField] GameObject[] m_ManagerList;

    public GameObject[] ManagerList => m_ManagerList;
}