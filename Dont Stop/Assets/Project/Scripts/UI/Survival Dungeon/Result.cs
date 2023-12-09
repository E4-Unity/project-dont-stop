using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    #region Initialization

    [Header("Initialization")]
    [SerializeField] GameObject m_GameResult;
    [SerializeField] GameObject m_NoticeGameClear;
    [SerializeField] GameObject m_NoticeGameOver;
    [SerializeField] Text m_TextClearTime;
    [SerializeField] Text m_TextExp;
    [SerializeField] Text m_TextGold;

    #endregion

    bool m_IsClear;

    #region Event Function

    void OnGameClear_Event()
    {
        m_NoticeGameClear.SetActive(true);
        m_IsClear = true;
    }

    void OnGameOver_Event()
    {
        m_NoticeGameOver.SetActive(true);
    }

    void OnGameFinish_Event()
    {
        // 게임 결과 창 표시
        m_GameResult.SetActive(true);
        
        // 전체 플레이 시간 표시
        var totalPlayTIme = GlobalGameManager.Instance.GetTimeManager().TotalPlayTime;
        var minutes = Mathf.FloorToInt(totalPlayTIme / 60);
        var seconds = Mathf.FloorToInt(totalPlayTIme % 60);
        m_TextClearTime.text = $"{minutes:D2}:{seconds:D2}";
        
        // 전체 획득 경험치 표시
        var killExp = SurvivalGameState.Instance.Exp;
        var bonusExp = m_IsClear ? Mathf.RoundToInt(killExp * 0.3f) : 0; // 게임 클리어 시 경험치 30% 추가 지급
        m_TextExp.text = $"EXP :{killExp}(+{bonusExp})";

        // 전체 획득 골드 표시
        var dropGold = SurvivalGameState.Instance.Gold;
        var bonusGold = m_IsClear ? dropGold / 2 : 0; // 게임 클리어 시 골드 50% 추가 지급
        m_TextGold.text = $"Gold:{dropGold}(+{bonusGold})";
    }

    #endregion

    #region Monobehaviour

    void OnEnable()
    {
        var gameManager = SurvivalGameManager.Get();
        gameManager.OnGameClear += OnGameClear_Event;
        gameManager.OnGameOver += OnGameOver_Event;
        gameManager.OnGameFinish += OnGameFinish_Event;
    }
    
    void OnDisable()
    {
        var gameManager = SurvivalGameManager.Get();
        if (gameManager is not null)
        {
            gameManager.OnGameClear -= OnGameClear_Event;
            gameManager.OnGameOver -= OnGameOver_Event;
            gameManager.OnGameFinish -= OnGameFinish_Event;
        }
    }

    #endregion
}
