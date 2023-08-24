using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBoss : MonoBehaviour
{
    static GameManagerBoss instance;
    public static GameManagerBoss Get() => instance;

    [Header("# Game Object")]
    public GameObject player;
    public PlayerBoss m_Player;
    public GameObject bossSlime;
    public GameObject spawnManager;
    public GameObject HUDBoss;
    public ResultBoss m_GameResult_UI;
    public GameObject bossHP;
    public Transform m_Joy_UI;
    public GameObject brickBack;
    public GameObject maze;
    public SelectBoss select;
    public Portal portal;

    public bool isGameOver = false;
    public bool portalTouch = false;
    public int eliteDead = 0;

    public PlayerBoss GetPlayer() => m_Player;

    [Header("# Game State")]
    [SerializeField, ReadOnly] bool m_IsPaused = false;
    [SerializeField, ReadOnly] float m_PlayTime = 0f;
    [SerializeField] public float m_MaxPlayTime = 600f;

    [Header("# Player State")]
    [SerializeField] int m_PlayerID;
    [SerializeField] float m_MaxHealth;
    [SerializeField, ReadOnly] float m_Health;
    
    [Header("# Boss State")]
    [SerializeField] float m_MaxBossHP = 30000f;
    [SerializeField, ReadOnly] float m_BossHP;

    [Header("# Elite State")]
    [SerializeField] float m_MaxEliteHP = 2000f;
    [SerializeField, ReadOnly] float m_EliteHP;

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
    public float BossHP
    {
        get => m_BossHP;
        set => m_BossHP = value;
    }
    public float MaxBossHP => m_MaxBossHP;

    public float EliteHP
    {
        get => m_EliteHP;
        set => m_EliteHP = value;
    }
    public float MaxEliteHP => m_MaxEliteHP;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        m_MaxHealth = 1000f;
        m_Health = m_MaxHealth;

        m_IsPaused = false;
        Time.timeScale = 1;
    }

    void Start()
    {
        m_Player.gameObject.SetActive(true);
        spawnManager.SetActive(true);
        HUDBoss.SetActive(true);

        portal = GameObject.Find("Portal").GetComponent<Portal>();

        GameStart();
    }

    public void GameStart()
    {
        if (GameStateBoss.Get().GetEquipmentComponent().WeaponData.WeaponType == EWeaponType.Melee)
            select.Select(0);
        else
            select.Select(1);

        ResumeGame();

        AudioManager.Get().PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Get().PlayBgm(true);
    }

    public void GameVictory()
    {
        isGameOver = true;
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        m_IsPaused = true;

        yield return new WaitForSeconds(0.5f);

        m_GameResult_UI.Win();
        PauseGame();

        AudioManager.Get().PlaySfx(AudioManager.Sfx.Win);
        AudioManager.Get().PlayBgm(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        m_IsPaused = true;

        yield return new WaitForSeconds(0.5f);

        m_GameResult_UI.Lose();
        PauseGame();

        AudioManager.Get().PlaySfx(AudioManager.Sfx.Lose);
        AudioManager.Get().PlayBgm(false);
    }

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

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        // 게임 정지
        if (IsPaused) return;

        m_PlayTime += Time.deltaTime;

        if (m_PlayTime > m_MaxPlayTime)
        {
            m_PlayTime = m_MaxPlayTime;
            GameOver();
        }

        if (eliteDead >= 3)
        {
            portal.activate = true;
        }

        if (portalTouch == true)
        {
            SetBossStage();

            m_BossHP = GameObject.FindWithTag("Boss").GetComponent<BossSlime>().BossSlimeHp;
        }
    }

    void SetBossStage()
    {
        maze.SetActive(false);
        
        brickBack.SetActive(true);
        bossHP.SetActive(true);
        bossSlime.SetActive(true);
    }
}
