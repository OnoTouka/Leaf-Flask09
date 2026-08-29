using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public static ItemList Instance;

    // アイテム一覧が変更されたときに通知
    public event Action OnInventoryChanged;

    // ランダム獲得するアイテム
    private List<ItemData> itemPool =
        new List<ItemData>();

    // 所持アイテム
    private List<ItemData> obtainedItems =
        new List<ItemData>();

    // 所持個数
    private List<int> obtainedAmounts =
        new List<int>();

    // ItemListを取得
    // なければ自動生成
    public static ItemList GetInstance()
    {
        if (Instance != null)
        {
            return Instance;
        }


        GameObject obj =
            new GameObject("ItemList");


        Instance =
            obj.AddComponent<ItemList>();


        DontDestroyOnLoad(obj);


        return Instance;
    }

    // 初期化
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

    // ItemDataを自動読み込み
    private void LoadItemData()
    {
        itemPool.Clear();


        ItemData[] items =
            Resources.LoadAll<ItemData>(
                "ItemData"
            );


        if (items == null ||
            items.Length == 0)
        {
            Debug.LogWarning(
                "Resources/ItemData に " +
                "ItemDataがありません"
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

    // ランダムアイテム獲得
    public ItemData GetRandomItem(
        out int amount)
    {
        amount = 0;


        if (itemPool.Count == 0)
        {
            Debug.LogWarning(
                "Item Poolにアイテムが登録されていません"
            );

            return null;
        }


        // ランダムでアイテムを選択
        int randomIndex =
            UnityEngine.Random.Range(
                0,
                itemPool.Count
            );


        ItemData item =
            itemPool[randomIndex];


        // 個数を決定
        amount =
            UnityEngine.Random.Range(
                item.minAmount,
                item.maxAmount + 1
            );


        // すでに所持しているか
        int index =
            obtainedItems.IndexOf(item);


        if (index >= 0)
        {
            // すでに持っている
            obtainedAmounts[index] += amount;
        }
        else
        {
            // 新しく追加
            obtainedItems.Add(item);

            obtainedAmounts.Add(amount);
        }


        // InventoryUIへ変更を通知
        OnInventoryChanged?.Invoke();


        return item;
    }

    // 所持アイテム一覧
    public List<ItemData> GetObtainedItems()
    {
        return obtainedItems;
    }

    // 所持個数一覧
    public List<int> GetObtainedAmounts()
    {
        return obtainedAmounts;
    }

    // 特定アイテムの所持数
    public int GetAmount(
        ItemData item)
    {
        int index =
            obtainedItems.IndexOf(item);


        if (index >= 0)
        {
            return obtainedAmounts[index];
        }


        return 0;
    }

    // アイテムを減らす
    public void RemoveItem(
        ItemData item,
        int amount)
    {
        int index =
            obtainedItems.IndexOf(item);


        if (index < 0)
        {
            return;
        }


        obtainedAmounts[index] -= amount;


        if (obtainedAmounts[index] <= 0)
        {
            obtainedItems.RemoveAt(index);

            obtainedAmounts.RemoveAt(index);
        }


        // 変更を通知
        OnInventoryChanged?.Invoke();
    }

    // 全アイテム削除
    public void ClearObtainedItems()
    {
        obtainedItems.Clear();

        obtainedAmounts.Clear();


        //  変更を通知
        OnInventoryChanged?.Invoke();
    }
}