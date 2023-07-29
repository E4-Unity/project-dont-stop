using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    /* 컴포넌트 */
    RectTransform m_RectTransform;
    Item[] m_Items;

    protected RectTransform GetRectTransform() => m_RectTransform;

    /* 메서드 */
    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (var item in m_Items)
        {
            item.gameObject.SetActive(false);
        }
        
        // 2. 그 중에서 랜덤하게 3개의 아이템만 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, m_Items.Length);
            ran[1] = Random.Range(0, m_Items.Length);
            ran[2] = Random.Range(0, m_Items.Length);

            if (ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2]) 
                break;
        }

        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = m_Items[ran[i]];
            
            // 3. 만렙 아이템의 경우는 소비 아이템으로 대체
            if (ranItem.Level == ranItem.Data.Damages.Length)
            {
                m_Items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
    /* API */
    public void Show()
    {
        Next();
        m_RectTransform.localScale = Vector3.one;
        GameManager.Get().PauseGame();
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.Get().EffectBgm(true);
    }

    public void Hide()
    {
        m_RectTransform.localScale = Vector3.zero;
        GameManager.Get().ResumeGame();
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Get().EffectBgm(false);
    }

    public void Select(int _index)
    {
        m_Items[_index].OnClick();
    }

    /* MonoBehaviour */
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Items = GetComponentsInChildren<Item>(true);
    }
}
