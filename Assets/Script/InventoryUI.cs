using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("アイテムを並べる場所")]
    [SerializeField]
    private Transform content;


    [Header("アイテムスロットPrefab")]
    [SerializeField]
    private GameObject itemSlotPrefab;


    private void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        // 古い表示を削除
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }


        // ItemListを取得
        // なければ自動生成
        ItemList itemList =
            ItemList.GetInstance();


        var items =
            itemList.GetObtainedItems();


        var amounts =
            itemList.GetObtainedAmounts();


        // ID順に並べる
        var sortedItems =
            items
            .Select((item, index) => new
            {
                item = item,
                amount = amounts[index]
            })
            .OrderBy(x => x.item.itemID)
            .ToList();


        // アイテムを生成
        foreach (var data in sortedItems)
        {
            GameObject slot =
                Instantiate(
                    itemSlotPrefab,
                    content
                );


            ItemSlotUI slotUI =
                slot.GetComponent<ItemSlotUI>();


            if (slotUI != null)
            {
                slotUI.SetItem(
                    data.item,
                    data.amount
                );
            }
            else
            {
                Debug.LogError(
                    "ItemSlotPrefabに" +
                    "ItemSlotUIがありません"
                );
            }
        }
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }
}