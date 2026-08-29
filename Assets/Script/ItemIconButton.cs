using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconButton :
    MonoBehaviour,
    IPointerClickHandler
{
    private ItemData item;
    private int amount;


    public void Setup(
        ItemData itemData,
        int itemAmount)
    {
        item = itemData;
        amount = itemAmount;
    }


    public void OnPointerClick(
        PointerEventData eventData)
    {
        Debug.Log(
            "アイコンをクリックしました：" +
            item.itemName
        );


        if (item == null)
        {
            Debug.LogError(
                "ItemDataがありません"
            );

            return;
        }


        if (ItemDetailUI.Instance == null)
        {
            Debug.LogError(
                "ItemDetailUIが存在しません"
            );

            return;
        }


        ItemDetailUI.Instance.ShowItem(
            item,
            amount
        );
    }
}