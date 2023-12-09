using Framework;
using UnityEngine;
using UnityEngine.UI;

public interface IShopItem
{
    public int Cost { get; }
    public Sprite Icon { get; }
    public string DisplayName { get; }
}

public class ShopItem : MonoBehaviour
{
    /* 레퍼런스 */
    [Header("UI")]
    [SerializeField] Image m_ImageIcon;
    [SerializeField] Text m_TextName;
    [SerializeField] Text m_TextCost;

    [Header("Config")]
    [SerializeField] AttributeDefinitionBase m_ItemDefinition;
    
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    UISoundManager m_UISoundManager;
    
    /* 필드 */
    IShopItem m_ShopItem;
    
    /* MonoBehaviour */
    void Awake()
    {
        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
        m_UISoundManager = GlobalGameManager.Instance.GetUISoundManager();
    }
    
    void OnEnable()
    {
        // TODO 제거
        // null 체크
        OnValidate();

        // 이미 구매한 상품이면 제거
        var inventory = m_PlayerState.GetInventoryComponent();
        if(inventory.CheckItem(m_ItemDefinition))
            Destroy(gameObject);
        
        var equipmentComponent = m_PlayerState.GetEquipmentComponent();
        if(equipmentComponent.CheckItem(m_ItemDefinition))
            Destroy(gameObject);

        // TODO 빌드 시 작동 안 함
        // 금액이 부족하면 빨간 글씨로 가격 표시
        if(inventory.Gold < m_ShopItem.Cost)
            m_TextCost.color = Color.red;
        else
            m_TextCost.color = Color.white;
    }
    
    void OnValidate()
    {
        if (m_ItemDefinition is IShopItem shopItem)
        {
            if (m_ShopItem == shopItem) return;
            
            m_ShopItem = shopItem;
            
            // UI 갱신
            Refresh();
        }
        else
        {
            m_ItemDefinition = null;
            m_ShopItem = null;
            
            m_ImageIcon.sprite = null;
            m_TextName.text = "???";
            m_TextCost.text = "???";
        }
    }

    /* API */
    public void Buy()
    {
        // null 체크
        if (m_ShopItem is null) return;
        
        // 구매
        var inventory = m_PlayerState.GetInventoryComponent();
        
        if (inventory.Gold >= m_ShopItem.Cost)
        {
            if (inventory.AddItem(m_ItemDefinition))
            {
                inventory.Gold -= m_ShopItem.Cost;
                m_UISoundManager.PlaySound(EUISoundType.SelectButton);
                Destroy(gameObject);
            }
        }
    }

    /* 메서드 */

    // UI 갱신
    void Refresh()
    {
        var cost = m_ShopItem.Cost;
        
        m_ImageIcon.sprite = m_ShopItem.Icon;
        m_TextName.text = m_ShopItem.DisplayName;
        m_TextCost.text = cost >= 1000 ? $"{cost / 1000f:F1}K" : cost.ToString();
    }
}
