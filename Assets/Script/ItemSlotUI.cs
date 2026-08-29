using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    [Header("アイコンを表示する場所")]
    [SerializeField]
    private Transform iconParent;


    public void SetItem(
        ItemData item,
        int amount)
    {
        if (item == null)
        {
            return;
        }


        if (iconParent == null)
        {
            Debug.LogError(
                "Icon Parentが設定されていません"
            );

            return;
        }


        // 古いアイコンを削除
        foreach (Transform child in iconParent)
        {
            Destroy(child.gameObject);
        }


        // アイコンPrefab確認
        if (item.iconPrefab == null)
        {
            Debug.LogWarning(
                item.itemName +
                " にIcon Prefabが設定されていません"
            );

            return;
        }


        // アイコン生成
        GameObject icon =
            Instantiate(
                item.iconPrefab,
                iconParent
            );


        // RectTransform
        RectTransform rect =
            icon.GetComponent<RectTransform>();


        if (rect != null)
        {
            rect.localPosition =
                Vector3.zero;

            rect.localRotation =
                Quaternion.identity;

            rect.localScale =
                Vector3.one;
        }


        // 個数表示
        ItemIconUI iconUI =
            icon.GetComponent<ItemIconUI>();


        if (iconUI != null)
        {
            iconUI.SetAmount(amount);
        }


        // クリック処理
        ItemIconButton button =
            icon.GetComponent<ItemIconButton>();


        if (button == null)
        {
            button =
                icon.AddComponent<ItemIconButton>();
        }


        button.Setup(
            item,
            amount
        );
    }
}