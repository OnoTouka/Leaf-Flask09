using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Player SP")]
    public int maxSP = 100;
    public int currentSP;

    // バトル開始時のSP
    public int battleStartSP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            currentHP = maxHP;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // HPを減らす
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }
    }

    // HPを最大値に戻す
    public void ResetHP()
    {
        currentHP = maxHP;
    }

    // SPを増やす
    public void AddSP(int amount)
    {
        currentSP += amount;
        currentSP = Mathf.Clamp(currentSP, 0, maxSP);
    }

    // SPを減らす
    public void SubtractSP(int amount)
    {
        currentSP -= amount;
        currentSP = Mathf.Clamp(currentSP, 0, maxSP);
    }

    // バトル開始時のSPを記録
    public void StartBattle()
    {
        battleStartSP = currentSP;
    }
}