using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public static ItemList Instance;

    public event Action OnInventoryChanged;


    // =====================================================
    // アイテムプール
    // =====================================================

    private List<ItemData> itemPool =
        new List<ItemData>();


    // =====================================================
    // 全体の所持アイテム
    // ゲームをまたいで保持する
    // =====================================================

    private List<ItemData> obtainedItems =
        new List<ItemData>();

    private List<int> obtainedAmounts =
        new List<int>();


    // =====================================================
    // 今回のゲームで獲得したアイテム
    // START ～ ResultSceneまで
    // =====================================================

    private List<ItemData> gameObtainedItems =
        new List<ItemData>();

    private List<int> gameObtainedAmounts =
        new List<int>();


    // =====================================================
    // Instance取得
    // =====================================================

    public static ItemList GetInstance()
    {
        if (Instance != null)
            return Instance;


        GameObject obj =
            new GameObject("ItemList");

        Instance =
            obj.AddComponent<ItemList>();

        DontDestroyOnLoad(obj);

        return Instance;
    }


    // =====================================================
    // Awake
    // =====================================================

    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;

        DontDestroyOnLoad(gameObject);

        LoadItemData();
    }


    // =====================================================
    // ItemDataを読み込む
    // Resources/ItemData
    // =====================================================

    private void LoadItemData()
    {
        itemPool.Clear();


        ItemData[] items =
            Resources.LoadAll<ItemData>("ItemData");


        if (items == null ||
            items.Length == 0)
        {
            Debug.LogWarning(
                "Resources/ItemData に ItemDataがありません"
            );

            return;
        }


        foreach (ItemData item in items)
        {
            if (item != null)
            {
                itemPool.Add(item);
            }
        }


        Debug.Log(
            "ItemDataを " +
            itemPool.Count +
            "個読み込みました"
        );
    }


    // =====================================================
    // ランダムアイテム獲得
    // =====================================================

    public ItemData GetRandomItem(out int amount)
    {
        amount = 0;


        // -------------------------------------------------
        // Item Pool確認
        // -------------------------------------------------

        if (itemPool.Count == 0)
        {
            Debug.LogWarning(
                "Item Poolにアイテムが登録されていません"
            );

            return null;
        }


        // -------------------------------------------------
        // GameManager確認
        // -------------------------------------------------

        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "GameManagerが見つかりません"
            );

            return null;
        }


        // -------------------------------------------------
        // 現在の探索深度を取得
        // -------------------------------------------------

        int currentDepth =
            GameManager.Instance.explorationDepth;


        Debug.Log(
            "アイテム抽選時の探索深度：" +
            currentDepth
        );


        // =================================================
        // 現在の探索深度で獲得可能なアイテムだけを集める
        // =================================================

        List<ItemData> availableItems =
            new List<ItemData>();


        foreach (ItemData item in itemPool)
        {
            if (item == null)
                continue;


            // -------------------------------------------------
            // 探索深度範囲チェック
            // minDepth ～ maxDepthの間なら獲得可能
            // -------------------------------------------------

            if (currentDepth >= item.minDepth &&
                currentDepth <= item.maxDepth)
            {
                availableItems.Add(item);


                Debug.Log(
                    "獲得候補：" +
                    item.itemName +
                    " / 探索深度範囲：" +
                    item.minDepth +
                    "～" +
                    item.maxDepth
                );
            }
        }


        // =================================================
        // 獲得可能なアイテムがない
        // =================================================

        if (availableItems.Count == 0)
        {
            Debug.Log(
                "現在の探索深度 " +
                currentDepth +
                " では獲得できるアイテムがありません"
            );

            return null;
        }


        // =================================================
        // 獲得可能なアイテムからランダム選択
        // =================================================

        int randomIndex =
            UnityEngine.Random.Range(
                0,
                availableItems.Count
            );


        ItemData selectedItem =
            availableItems[randomIndex];


        // =================================================
        // 獲得個数を決定
        // =================================================

        amount =
            UnityEngine.Random.Range(
                selectedItem.minAmount,
                selectedItem.maxAmount + 1
            );


        // =================================================
        // 全体の所持アイテムに追加
        // =================================================

        int index =
            obtainedItems.IndexOf(selectedItem);


        if (index >= 0)
        {
            obtainedAmounts[index] += amount;
        }
        else
        {
            obtainedItems.Add(selectedItem);

            obtainedAmounts.Add(amount);
        }


        // =================================================
        // 今回のゲームで獲得したアイテムに追加
        // =================================================

        int gameIndex =
            gameObtainedItems.IndexOf(selectedItem);


        if (gameIndex >= 0)
        {
            gameObtainedAmounts[gameIndex] += amount;
        }
        else
        {
            gameObtainedItems.Add(selectedItem);

            gameObtainedAmounts.Add(amount);
        }


        // =================================================
        // インベントリ更新通知
        // =================================================

        OnInventoryChanged?.Invoke();


        // =================================================
        // デバッグ表示
        // =================================================

        Debug.Log(
            "アイテム獲得：" +
            selectedItem.itemName +
            " / 個数：" +
            amount +
            " / 現在の探索深度：" +
            currentDepth +
            " / 獲得可能深度：" +
            selectedItem.minDepth +
            "～" +
            selectedItem.maxDepth
        );


        return selectedItem;
    }


    // =====================================================
    // 全体の所持アイテム取得
    // =====================================================

    public List<ItemData> GetObtainedItems()
    {
        return obtainedItems;
    }


    // =====================================================
    // 全体の所持数取得
    // =====================================================

    public List<int> GetObtainedAmounts()
    {
        return obtainedAmounts;
    }


    // =====================================================
    // 今回のゲームで獲得したアイテム取得
    // =====================================================

    public List<ItemData> GetGameObtainedItems()
    {
        return gameObtainedItems;
    }


    // =====================================================
    // 今回のゲームで獲得した個数取得
    // =====================================================

    public List<int> GetGameObtainedAmounts()
    {
        return gameObtainedAmounts;
    }


    // =====================================================
    // 特定アイテムの全体所持数
    // =====================================================

    public int GetAmount(ItemData item)
    {
        int index =
            obtainedItems.IndexOf(item);


        if (index >= 0)
        {
            return obtainedAmounts[index];
        }


        return 0;
    }


    // =====================================================
    // アイテムを減らす
    // =====================================================

    public void RemoveItem(
        ItemData item,
        int amount
    )
    {
        int index =
            obtainedItems.IndexOf(item);


        if (index < 0)
            return;


        obtainedAmounts[index] -= amount;


        if (obtainedAmounts[index] <= 0)
        {
            obtainedItems.RemoveAt(index);

            obtainedAmounts.RemoveAt(index);
        }


        OnInventoryChanged?.Invoke();
    }


    // =====================================================
    // 全体の所持アイテムをクリア
    // ※通常はゲーム開始時に呼ばない
    // =====================================================

    public void ClearObtainedItems()
    {
        obtainedItems.Clear();

        obtainedAmounts.Clear();

        OnInventoryChanged?.Invoke();
    }


    // =====================================================
    // 今回のゲームで獲得したアイテムだけクリア
    // =====================================================

    public void ClearGameObtainedItems()
    {
        gameObtainedItems.Clear();

        gameObtainedAmounts.Clear();

        OnInventoryChanged?.Invoke();
    }
}