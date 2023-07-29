using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // static
    // 싱글톤
    static GameManager Instance;
    public static GameManager Get() => Instance;
    
    /* 레퍼런스 */
    [Header("# Game Object")]
    [SerializeField] Player m_Player;
    [SerializeField] PoolManager m_PoolManager;
    [SerializeField] LevelUp m_LevelUp_UI;
    [SerializeField] Result m_GameResult_UI;
    [SerializeField] GameObject m_EnemyCleaner;
    [SerializeField] Transform m_Joy_UI;

    public Player GetPlayer() => m_Player;
    public PoolManager GetPoolManager() => m_PoolManager;
    protected LevelUp GetLevelUp() => m_LevelUp_UI;

    /* 필드 */
    [Header("# Game State")]
    [SerializeField, ReadOnly] bool m_IsPaused = false;
    [SerializeField, ReadOnly] float m_PlayTime;
    [SerializeField] float m_MaxPlayTime = 20f;

    [Header("# Player State")] 
    [SerializeField] int m_PlayerID;
    [SerializeField] float m_MaxHealth = 100;
    [SerializeField, ReadOnly] float m_Health;
    [SerializeField, ReadOnly] int m_Level;
    [SerializeField, ReadOnly] int m_Kill;
    [SerializeField, ReadOnly] int m_Exp;
    [SerializeField] int[] m_NextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    /* 프로퍼티 */
    public bool IsPaused => m_IsPaused;
    public float PlayTime => m_PlayTime;
    public float MaxPlayTime => m_MaxPlayTime;
    public int PlayerID => m_PlayerID;
    public float Health
    {
        get => m_Health;
        set => m_Health = value;
    }
    public float MaxHealth => m_MaxHealth;
    public int Level => m_Level;
    public int Kill => m_Kill;
    public int Exp => m_Exp;
    public int NextExp => m_NextExp[Mathf.Min(m_Level, m_NextExp.Length - 1)];

    /* 메서드 */
    public void PauseGame()
    {
        m_IsPaused = true;
        Time.timeScale = 0;
        m_Joy_UI.localScale = Vector3.zero;
    }

    public void ResumeGame()
    {
        m_IsPaused = false;
        Time.timeScale = 1;
        m_Joy_UI.localScale = Vector3.one;
    }
    
    /* API */
    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void GameStart(int _characterID)
    {
        m_PlayerID = _characterID;
        
        m_Health = m_MaxHealth;

        m_Player.gameObject.SetActive(true);
        //TODO 임시로 캐릭터에게 무기를 쥐어줌
        m_LevelUp_UI.Select(m_PlayerID % 2);

        ResumeGame();
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Get().PlayBgm(true);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    // Dead 애니메이션이 재생되고 난 뒤에 게임이 멈춰야 하므로 딜레이를 준다.
    IEnumerator GameOverRoutine()
    {
        m_IsPaused = true;
        
        yield return new WaitForSeconds(0.5f);
        
        m_GameResult_UI.gameObject.SetActive(true);
        m_GameResult_UI.Lose();
        PauseGame();
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Lose);
        AudioManager.Get().PlayBgm(false);
    }
    
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    // Dead 애니메이션이 재생되고 난 뒤에 게임이 멈춰야 하므로 딜레이를 준다.
    IEnumerator GameVictoryRoutine()
    {
        m_IsPaused = true;
        m_EnemyCleaner.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        
        m_GameResult_UI.gameObject.SetActive(true);
        m_GameResult_UI.Win();
        PauseGame();
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Win);
        AudioManager.Get().PlayBgm(false);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    // Player
    public void GetExp()
    {
        if (m_IsPaused) return;
        
        m_Exp++; 
        m_Kill++;

        if (m_Exp == NextExp)
        {
            m_Exp = 0;
            m_Level++;
            m_LevelUp_UI.Show();
        }
    }

    /* MonoBehaviour */
    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        
        //TODO 임시
        m_IsPaused = true;
    }

    void Update()
    {
        // 게임 정지
        if (IsPaused) return;

        m_PlayTime += Time.deltaTime;

        if (m_PlayTime > m_MaxPlayTime)
        {
            m_PlayTime = m_MaxPlayTime;
            GameVictory();
        }
    }
}
