using System.Collections;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_AlphaSpeed;
    [SerializeField] float m_DisappearTime;

    float m_Damage;
    Vector3 m_SpawnPosition;
    
    TextMesh m_FloatingTextDamage;
    Color m_OriginalColor;
    Color m_ChangedColor;
    
    // buffer
    IEnumerator disappearCoroutine;

    public void Init(float _damage, Vector3 _spawnPosition)
    {
        m_Damage = _damage;
        m_SpawnPosition = _spawnPosition;
    }

    void Awake()
    {
        m_FloatingTextDamage = GetComponent<TextMesh>();
        m_OriginalColor = m_FloatingTextDamage.color;
        disappearCoroutine = Disappear();
    }

    void OnEnable()
    {
        m_ChangedColor = m_OriginalColor;
        transform.position = m_SpawnPosition;
        m_FloatingTextDamage.text = $"{m_Damage}";
        StartCoroutine(disappearCoroutine);
        StartCoroutine(Destroy());
    }

    IEnumerator Disappear()
    {
        while (true)
        {
            yield return null;
            transform.position += new Vector3(0, m_MoveSpeed * Time.deltaTime, 0);
            m_ChangedColor.a = Mathf.Lerp(m_ChangedColor.a, 0, Time.deltaTime * m_AlphaSpeed);
            m_FloatingTextDamage.color = m_ChangedColor;
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(m_DisappearTime);
        StopCoroutine(disappearCoroutine);
        PoolManager.ReleaseInstance(gameObject);
    }
    
}
