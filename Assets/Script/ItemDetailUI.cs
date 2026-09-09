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

    public void ShowItem(ItemData item, int amount)
    {
        if (item == null)
        {
            Debug.LogWarning("ItemDataがありません");
            return;
        }

        gameObject.SetActive(true);
        transform.localScale = Vector3.one;

        if (itemNameText != null)
            itemNameText.text = item.itemName;

        if (idText != null)
            idText.text = item.itemID.ToString();

        if (amountText != null)
            amountText.text = amount.ToString();

        if (descriptionText != null)
            descriptionText.text = item.description;

        if (illustrationParent != null)
        {
            foreach (Transform child in illustrationParent)
            {
                Destroy(child.gameObject);
            }
        }

        if (item.illustrationPrefab != null &&
            illustrationParent != null)
        {
            GameObject illustration =
                Instantiate(
                    item.illustrationPrefab,
                    illustrationParent
                );

            RectTransform rect =
                illustration.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.localPosition = Vector3.zero;
                rect.localRotation = Quaternion.identity;
                rect.localScale = Vector3.one;
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