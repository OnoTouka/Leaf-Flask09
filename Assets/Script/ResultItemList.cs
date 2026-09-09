using System.Collections.Generic;
using UnityEngine;

public class ResultItemList : MonoBehaviour
{
    [Header("アイコンを並べる場所")]
    [SerializeField]
    private Transform content;


    // =====================================================
    // Start
    // =====================================================

    private void Start()
    {
        CreateItemList();
    }


    // =====================================================
    // アイテム一覧を生成
    // =====================================================

    private void CreateItemList()
    {
        if (content == null)
        {
            Debug.LogWarning(
                "Contentが設定されていません"
            );

            return;
        }


        // =================================================
        // ItemListを取得
        // =================================================

        ItemList itemList =
            ItemList.GetInstance();


        // =================================================
        // 今回のゲームで獲得したアイテムを取得
        // =================================================

        List<ItemData> items =
            itemList.GetGameObtainedItems();


        List<int> amounts =
            itemList.GetGameObtainedAmounts();


        // =================================================
        // アイテムを生成
        // =================================================

        for (int i = 0; i < items.Count; i++)
        {
            ItemData item =
                items[i];


            if (item == null)
            {
                continue;
            }


            if (item.iconPrefab == null)
            {
                Debug.LogWarning(
                    item.itemName +
                    " に一覧用アイコンが設定されていません"
                );

                continue;
            }


            // =================================================
            // アイテムPrefabを生成
            // =================================================

            GameObject icon =
                Instantiate(
                    item.iconPrefab,
                    content
                );


            // =================================================
            // 今回のゲームで獲得した個数を表示
            // =================================================

            ItemIconUI itemIconUI =
                icon.GetComponentInChildren<ItemIconUI>();


            if (itemIconUI != null)
            {
                itemIconUI.SetAmount(
                    amounts[i]
                );
            }
        }
    }
}