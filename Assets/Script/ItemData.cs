using UnityEngine;

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Game/Item Data"
)]
public class ItemData : ScriptableObject
{
    [Header("アイテムID")]
    public int itemID;


    [Header("アイテム情報")]
    public string itemName;

    [TextArea(3, 5)]
    public string description;


    [Header("一覧用アイコン")]
    public GameObject iconPrefab;


    [Header("詳細画面用イラスト")]
    public GameObject illustrationPrefab;


    [Header("獲得個数")]
    [Min(1)]
    public int minAmount = 1;

    [Min(1)]
    public int maxAmount = 3;


    // =====================================================
    // 獲得可能な探索深度
    // =====================================================

    [Header("獲得可能な探索深度")]
    [Min(0)]
    public int minDepth = 0;

    [Min(0)]
    public int maxDepth = 10;
}