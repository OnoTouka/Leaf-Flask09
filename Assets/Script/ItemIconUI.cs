using TMPro;
using UnityEngine;

public class ItemIconUI : MonoBehaviour
{
    [Header("Prefab内の個数表示")]
    [SerializeField]
    private TMP_Text amountText;


    public void SetAmount(int amount)
    {
        if (amountText == null)
        {
            Debug.LogWarning(
                "ItemIconUIのAmount Textが設定されていません"
            );

            return;
        }

        amountText.text = amount.ToString();
    }
}