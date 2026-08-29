using TMPro;
using UnityEngine;

public class ItemDetailUI : MonoBehaviour
{
    public static ItemDetailUI Instance;


    [Header("詳細画面用イラスト")]
    [SerializeField]
    private Transform illustrationParent;


    [Header("アイテム名")]
    [SerializeField]
    private TMP_Text itemNameText;


    [Header("アイテムID")]
    [SerializeField]
    private TMP_Text idText;


    [Header("個数")]
    [SerializeField]
    private TMP_Text amountText;


    [Header("説明")]
    [SerializeField]
    private TMP_Text descriptionText;


    // =====================================================
    // Awake
    // =====================================================

    private void Awake()
    {
        Instance = this;
    }


    // =====================================================
    // Start
    // =====================================================

    private void Start()
    {
        // 最初は非表示
        gameObject.SetActive(false);
    }


    // =====================================================
    // アイテム詳細を表示
    // =====================================================

    public void ShowItem(
        ItemData item,
        int amount)
    {
        if (item == null)
        {
            Debug.LogWarning(
                "ItemDataがありません"
            );

            return;
        }


        // =================================================
        // ダイアログを表示
        // =================================================

        gameObject.SetActive(true);


        // =================================================
        // アイテム名
        // =================================================

        if (itemNameText != null)
        {
            itemNameText.text =
                item.itemName;
        }


        // =================================================
        // アイテムID
        // =================================================

        if (idText != null)
        {
            idText.text =
                item.itemID.ToString();
        }


        // =================================================
        // 個数
        // =================================================

        if (amountText != null)
        {
            amountText.text =
                amount.ToString();
        }


        // =================================================
        // 説明
        // =================================================

        if (descriptionText != null)
        {
            descriptionText.text =
                item.description;
        }


        // =================================================
        // 古いイラストを削除
        // =================================================

        if (illustrationParent != null)
        {
            foreach (
                Transform child
                in illustrationParent
            )
            {
                Destroy(
                    child.gameObject
                );
            }
        }


        // =================================================
        // 詳細画面用イラストを生成
        // =================================================

        if (
            item.illustrationPrefab != null &&
            illustrationParent != null
        )
        {
            GameObject illustration =
                Instantiate(
                    item.illustrationPrefab,
                    illustrationParent
                );


            // =================================================
            // RectTransform調整
            // =================================================

            RectTransform rect =
                illustration.GetComponent<
                    RectTransform
                >();


            if (rect != null)
            {
                rect.localPosition =
                    Vector3.zero;

                rect.localRotation =
                    Quaternion.identity;

                rect.localScale =
                    Vector3.one;
            }
        }
        else
        {
            Debug.LogWarning(
                item.itemName +
                " に詳細画面用イラストが設定されていません"
            );
        }
    }
}