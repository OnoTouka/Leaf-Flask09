using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [Header("背景")]
    public GameObject normalBackground;
    public GameObject defeatBackground;

    private void Start()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.battleDefeat)
        {
            // 敗北
            normalBackground.SetActive(false);
            defeatBackground.SetActive(true);
        }
        else
        {
            // 勝利など
            normalBackground.SetActive(true);
            defeatBackground.SetActive(false);
        }
    }
}