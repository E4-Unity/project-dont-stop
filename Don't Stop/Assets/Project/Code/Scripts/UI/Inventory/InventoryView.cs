using UnityEngine;
using UnityEngine.UI;
using Presenter;

public class InventoryView : MonoBehaviour, IInventory
{
    [Header("Reference")]
    [SerializeField] Text m_GoldText;
    [SerializeField] Text m_CrystalText;
    
    /* 버퍼 */
    int gold;
    int crystal;

    public int Gold
    {
        set
        {
            gold = value;
            m_GoldText.text = gold >= 10000 ? $"{gold / 10000f:F1}K" : $"{gold}";
        }
    }
    public int Crystal
    {
        set
        {
            crystal = value;
            m_CrystalText.text = crystal >= 10000 ? $"{crystal / 10000f:F1}K" : $"{crystal}";
        }
    }
}
