using TMPro;
using UnityEngine;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("HP表示")]
    [SerializeField]
    private TMP_Text hpText;

    [Header("SP表示")]
    [SerializeField]
    private TMP_Text spText;

    [Header("深度表示")]
    [SerializeField]
    private TMP_Text depthText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (GameManager.Instance == null)
            return;

        if (hpText != null)
        {
            hpText.text =
                "HP " +
                GameManager.Instance.currentHP +
                " / " +
                GameManager.Instance.maxHP;
        }

        if (spText != null)
        {
            spText.text =
                "SP " +
                GameManager.Instance.currentSP +
                " / " +
                GameManager.Instance.maxSP;
        }

        if (depthText != null)
        {
            depthText.text =
                "深度 " +
                GameManager.Instance.explorationDepth;
        }
    }
}