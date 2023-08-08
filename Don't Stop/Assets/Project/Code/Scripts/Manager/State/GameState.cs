using System;
using UnityEditor;
using UnityEngine;

public abstract class GameState<T> : GenericMonoSingleton<T> where T : GameState<T>
{
    // 씬 로드 전에 생성될 매니저 리스트
    [SerializeField] GameObject m_ManagerGroup;
    [SerializeField] EManagerClass[] m_ManagerClasses;
    GameObject[] m_CreatedManagers;
    
    // 추가된 순서대로 매니저 게임 오브젝트를 SetActive(true)
    [SerializeField] GameObject[] m_GameObjectsToActivate;

    protected abstract void CopyPlayerState();
    protected abstract void UpdatePlayerState();

    void CreateManagers()
    {
        m_CreatedManagers = new GameObject[m_ManagerClasses.Length];
        var managerClasses = ManagerClassDictionary.Get(m_ManagerClasses);
        
        for (int i = 0; i < m_ManagerClasses.Length; i++)
        {
            // 매니저 생성
            var managerObject = new GameObject(managerClasses[i].Name);
            managerObject.AddComponent(managerClasses[i]);
            
            // 매니저 그룹이 설정되어 있다면 하위 계층으로 이동
            if (m_ManagerGroup)
                managerObject.transform.parent = m_ManagerGroup.transform;
            
            // 레퍼런스 저장
            m_CreatedManagers[i] = managerObject;
        }
        
        Debug.Log(GetType().Name + " > Create Managers");
    }

    void DestroyManagers()
    {
        var managerClasses = ManagerClassDictionary.Get(m_ManagerClasses);
        
        // 생성된 순서의 반대로 파괴
        for (int i = m_ManagerClasses.Length - 1; i >= 0; i--)
        {
            // 매니저 생성
            var managerObject = new GameObject(managerClasses[i].Name);
            managerObject.AddComponent(managerClasses[i]);
            
            // 매니저 그룹이 설정되어 있다면 하위 계층으로 이동
            if (m_ManagerGroup)
                managerObject.transform.parent = m_ManagerGroup.transform;
        }
        
        Debug.Log(GetType().Name + " > Destroy Managers");
    }
    
    void ActivateGameObjects()
    {
        for (int i = 0; i < m_GameObjectsToActivate.Length; i++)
        {
            m_GameObjectsToActivate[i].SetActive(true);
        }
        
        Debug.Log(GetType().Name + " > Activate Game Objects");
    }

    void DeactivateGameObjects()
    {
        for (int i = m_GameObjectsToActivate.Length - 1; i >= 0; i--)
        {
            m_GameObjectsToActivate[i].SetActive(false);
        }
        
        Debug.Log(GetType().Name + " > Deactivate Game Objects");
    }

    protected virtual void OnEnable()
    {
        CopyPlayerState();
        CreateManagers();
        ActivateGameObjects();
    }

    protected virtual void OnDisable()
    {
        DeactivateGameObjects();
        DestroyManagers();
        UpdatePlayerState();
    }
}
